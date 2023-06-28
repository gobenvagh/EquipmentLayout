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
    public class CsvDeviceDeserializer
    {

        /*public IList<Device> Read(string filename)
        {
            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HasHeaderRecord = false,
            };

            using (var reader = new StreamReader(filename))
            using (var csv = new CsvReader(reader, configuration))
            {
                var records = csv.GetRecords<DeviceDto>();
                var factory = new DeviceFactory();
                var templates = new List<DeviceTemplate>();
                var devices = new List<Device>();
                foreach (var record in records)
                {
                    var width = record.X2 - record.X;
                    var height = record.Y2 - record.Y;
                    var template = templates.FirstOrDefault(t => t.Width == width && t.Height == height);
                    if (template == null)
                    {
                        template = new DeviceTemplate(width, height, "1");
                        templates.Add(template);
                    }
                    var device = factory.GetDevice(new Point(record.X, record.Y), template);
                    devices.Add(device);
                }
                return devices;
            }
        }*/
    }
}