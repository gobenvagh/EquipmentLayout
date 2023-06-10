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

namespace EquipmentLayout.Models
{
    public class CsvDeviceSerializer
    {

        public void Write(Rectangle zoneDevice, IList<Device> records, string filename)
        {
            var zone = new DeviceDtoSize((int)zoneDevice.Width, (int)zoneDevice.Height, 1);
            var newRecords = records.Select(x => new DeviceDtoSize(x.Width, x.Height, 1));

            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                NewLine = Environment.NewLine,
            };

            using (var writer = new StreamWriter(filename))
            using (var csv = new CsvWriter(writer, configuration))
            {
                csv.WriteRecord(zone);
                csv.NextRecord();
                csv.WriteRecord(zone);
                csv.WriteRecords(newRecords);
                csv.Flush();
            }

        }
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


}