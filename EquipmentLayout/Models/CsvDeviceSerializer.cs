using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using EquipmentLayout.ViewModels;

namespace EquipmentLayout.Models
{
/*    public class CsvDeviceSerializer
    {
        public void Write(Rectangle zoneDevice, IList<DeviceTemplateViewModel> deviceTempRecords,
            IList<ObstacleViewModel> obstacleRecords, string filename)
        {
            var zone = new ZoneDto((int)zoneDevice.Width, (int)zoneDevice.Height);
            var deviceDtos = deviceTempRecords
                .Select(x => new DeviceDtoSize(x.Width, x.Height, x.Count));
            var obstacleDtos = obstacleRecords.
                Select(x => new ObstacleDto(x.Width, x.Height, x.X, x.Y));

            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                NewLine = Environment.NewLine,
                HasHeaderRecord= false,
            };

            using (var writer = new StreamWriter(filename))
            using (var csv = new CsvWriter(writer, configuration))
            {
                csv.WriteRecord(zone);
                csv.NextRecord();
                csv.WriteRecords(deviceDtos);
                csv.WriteRecords(obstacleDtos);
                csv.Flush();
            }
            
        }
    }*/

/*    class ZoneDto
    {
        public ZoneDto(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public int Width { get; set; }

        public int Height { get; set; }
    }

    class DeviceDtoSize
    {        
        public int Width { get; set; }

        public int Height { get; set; }

        public int Count { get; set; }

        public DeviceDtoSize(int width, int height, int count)
        {
            this.Width = width;
            this.Height = height;
            this.Count = count;
        }
    }

    class DeviceDto
    {
        [Index(0)]
        public int X { get; set; }
        [Index(1)]
        public int Y { get; set; }
        
        [Index(2)]
        public int X2 { get; set; }
        
        [Index(3)]
        public int Y2 { get; set; }
    }

    internal class ObstacleDto
    {
        [Index(0)]
        public int Width { get; set; }
        [Index(1)]
        public int Height { get; set; }
        [Index(2)]
        public int X { get; set; }
        [Index(3)]
        public int Y { get; set; }

        public ObstacleDto(int width, int height, int x, int y)
        {
            this.Width = width;
            this.Height = height;
            this.X = x;
            this.X = y;
        }
    }*/


}
