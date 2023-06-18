using EquipmentLayout.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace EquipmentLayout.Models
{
    internal class Solver
    {
        private class Corner
        {
            public int X;
            public int Y;
            public bool IsFree;
            public Corner(int x, int y)
            {
                X = x;
                Y = y;
                IsFree = true;
            }
        }

        private class RectInfo
        {
            public DeviceTemplateViewModel Model { get; set; }

            public Corner LT;
            public Corner RT;
            public Corner LB;
            public Corner RB;

            public int Width => RB.X - LT.X;

            public int Height  => RB.Y - LT.Y;

            public int[] ToArray()
            {
                return new int[] { LT.X, LT.Y };
            }

            public Rect ToRect()
            {
                return new Rect()
                {
                    X = this.LT.X,
                    Y = this.LT.Y,
                    Height = this.Height,
                    Width = this.Width,
                };
            }

            public void SetOnCorner(Corner corner)
            {
                var height = Height;
                var width = Width;

                LT.X = corner.X;
                LT.Y = corner.Y;

                LB.X = corner.X;
                LB.Y = corner.Y + height;

                RB.X = corner.X + width;                
                RB.Y = corner.Y + height;

                RT.X = corner.X + width;
                RT.Y = corner.Y;

            }

            public RectInfo(int width, int height)
            {
                LT = new Corner(0, 0);
                RT = new Corner(width, 0);
                LB = new Corner(0, height);
                RB = new Corner(width, height);
            }


            public RectInfo(int width, int height, int x, int y)
            {
                LT = new Corner(x, y);
                RT = new Corner(x + width, y);
                LB = new Corner(x, y + height);
                RB = new Corner(x + width, y +height);
            }
        }

        List<RectInfo> _devices;
        List<RectInfo> _obsts;

        bool IsIntersect(RectInfo left, RectInfo right)
        {
            Rect leftR = left.ToRect();
            Rect rightR = right.ToRect();
            leftR.Intersect(rightR);

            return !(leftR == Rect.Empty || leftR.Width == 0 || leftR.Height == 0);
        }


        private List<RectInfo> GenChildRects(List<DeviceTemplateViewModel> deviceTemplateViewModels)
        {
            var childRects = new List<RectInfo>();
            foreach (var temp in deviceTemplateViewModels)
            {
                for (int i = 0; i < temp.Count; i++)
                {
                    var list = new List<int[]>();
                    var model = temp.Model;
                    list.Add(new int[] { 0, 0, model.Width, model.Height });
                    list.Add(new int[]
                    {
                        model.ServiceArea.X,
                        model.ServiceArea.Y,
                        model.ServiceArea.X + model.ServiceArea.Width,
                        model.ServiceArea.Y +  model.ServiceArea.Height
                    });

                    list.Add(new int[]
                    {
                        model.WorkArea.X,
                        model.WorkArea.Y,
                        model.WorkArea.X + model.WorkArea.Width,
                        model.WorkArea.Y +  model.WorkArea.Height
                    });

                    var device = OuterRect(list);



                    var width = device[2] - device[0];
                    var height = device[3] - device[1];

                    childRects.Add(new RectInfo(width, height));
                }
            }

            return childRects;
        }


        int[] OuterRect(List<int[]> rects)
        {
            var x0 = rects.Min(r => r[0]);
            var y0 = rects.Min(r => r[1]);
            var x1 = rects.Max(r => r[2]);
            var y1 = rects.Max(r => r[3]);
            return new int[] { x0, y0, x1, y1 };
        }

        RectInfo GetPhantomDevice(int x, int y)
        {
            return new RectInfo(0, 0, x, y);
        }

        RectInfo GetPhantomDevice(Corner c)
        {
            return GetPhantomDevice(c.X, c.Y);
        }

        Corner TopCornerProjection(Corner a)
        {
            var device = _devices.Where(d => d.LT.X < a.X && d.RB.X > a.X)?.OrderBy(d => d.RB.Y)?.LastOrDefault();
            if (device is null)
                return new Corner(a.X, 0);
            return new Corner(a.X, device.RB.Y);
        }

        Corner LeftCornerProjection(Corner a)
        {
            var device = _devices.Where(d => d.LT.Y < a.Y && d.RB.Y > a.Y)?.OrderBy(d => d.RB.X)?.LastOrDefault();
            if (device is null)
                return new Corner(0, a.Y);
            return new Corner(device.RB.X, a.Y);
        }

        public List<int[]> PlaceEquipment(List<DeviceTemplateViewModel> devices, List<int[]> parentRects, List<Obstacle> obstacles)
        {
            _devices = new List<RectInfo>();
            var childRects = GenChildRects(devices);
            _obsts = obstacles.Select(o => new RectInfo(o.Width, o.Height, o.X, o.Y)).ToList();
            _devices.AddRange(_obsts);

            var phantomDevices = new List<RectInfo>();
            phantomDevices.Add(GetPhantomDevice(0, 0));

            phantomDevices.AddRange(_obsts.Select(o => GetPhantomDevice(LeftCornerProjection(o.LT))));
            phantomDevices.AddRange(_obsts.Select(o => GetPhantomDevice(TopCornerProjection(o.LT))));

            _devices.AddRange(phantomDevices);

            var parentRect = parentRects[0];
            foreach (var rect in childRects.OrderByDescending(r => r.Width + r.Height))
            {

                var device = rect;
                var isSet = SetType.CanSet;
                if (_devices.Count() == 0)
                {
                    device.SetOnCorner(new Corner(0,0));
                    isSet = IsCanSet(device, parentRect);
                    if (isSet == SetType.CanSet)
                    {
                        _devices.Add(device);
                        continue;
                    }
                }


                foreach (var dev in _devices.Where(d => d.RT.IsFree))
                {
                    device.SetOnCorner(dev.RT);
                    isSet = IsCanSet(device, parentRect);
                    if (isSet == SetType.CanSet)
                    {
                        dev.RT.IsFree = false;
                        break;
                    }
                }

                if (isSet == SetType.CanSet)
                {
                    _devices.Add(device);
                    continue;
                }

                foreach (var dev in _devices.Where(d => d.LB.IsFree))
                {
                    device.SetOnCorner(dev.LB);
                    isSet = IsCanSet(device, parentRect);
                    if (isSet == SetType.CanSet )
                    {
                        dev.LB.IsFree = false;
                        break;
                    }
                }

                if (isSet == SetType.CanSet)
                {
                    _devices.Add(device);
                    continue;
                }

                foreach (var dev in _devices.Where(d => d.RB.IsFree))
                {
                    device.SetOnCorner(dev.RB);
                    isSet = IsCanSet(device, parentRect);
                    if (isSet == SetType.CanSet)
                    {
                        dev.RB.IsFree = false;
                        break;
                    }
                }


                if (isSet == SetType.CanSet)
                {
                    _devices.Add(device);
                    continue;
                }

                if (isSet != SetType.CanSet)
                {
                    throw new InvalidOperationException("(((");
                }

            }

            _devices.RemoveAll(d=>_obsts.Contains(d));  
            _devices.RemoveAll(d=> phantomDevices.Contains(d));  

            return _devices.Select(x=>x.ToArray()).ToList();
        }

        enum SetType
        {
            CanSet,
            ObstIntersect,
            DeviceIntesect,
            NotInParent
        }

        private SetType IsCanSet(RectInfo device, int[] parentRect)
        {
            // Проверяем на пересечение с препятствиями
            foreach (var obst in _obsts)
            {
                if (IsIntersect(device, obst))
                    return SetType.ObstIntersect;
            }
            
            foreach (var set in _devices)
            {
                if(set.Width == 0 || set.Height == 0)
                    continue;
                if (IsIntersect(device, set))
                    return SetType.DeviceIntesect;
            }

            if (!IsNotInParent(device, parentRect))
                return SetType.NotInParent;

            return SetType.CanSet;
        }

        private bool IsNotInParent(RectInfo device, int[] parentRect)
        {
            return device.LT.X >= 0 && device.LT.Y >= 0 && device.RB.X <= parentRect[0] && device.RB.Y <= parentRect[1];
        }
    }
}
