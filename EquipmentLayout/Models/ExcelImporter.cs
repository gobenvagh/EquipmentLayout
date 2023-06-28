using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.IO;
using EquipmentLayout.ViewModels;
using CsvHelper;

namespace EquipmentLayout.Models
{

    public class DeviceDto
    {
        public string ZoneName { get; set; }
        public string TemplateType { get; set; }
        public int X{ get; set; }
        public int Y{ get; set; }

    }

    public class ObstacleDto
    {
        public string ZoneName { get; set; }
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

    }

    public class ImportResult
    {
        public List<ProvZoneViewModel> Zones { get; set; }

        public List<DeviceTemplateViewModel> Templates { get; set; }
    }

    public class ExcelImporter
    {

        public ImportResult ImportWithExcel(string filePath)
        {
            var templates = ImportDeviceTemplatesFromExcel(filePath);
            var deviceDtos = ImportDevicesFromExcel(filePath);
            var zones = ImportZonesFromExcel(filePath);
            var obstcls = ImportObstaclesFromExcel(filePath);

            List<Device> devices = new List<Device>();

            for(int i = 0; i < deviceDtos.Count(); i++)
            {
                var template = templates.FirstOrDefault(t => t.Name == deviceDtos[i].TemplateType);
                var device = new DeviceFactory().GetDevice(deviceDtos[i].X, deviceDtos[i].Y, template, false);

                var vm = new DeviceViewModel(device);

                zones.FirstOrDefault(z => z.Name == deviceDtos[i].ZoneName).RectItems.Add(vm);
            }

            for(int i = 0; i < obstcls.Count(); i++)
            {
                var obs = obstcls[i];
                var vm = new ObstacleViewModel(obs.X, obs.Y, obs.Width, obs.Height);
                zones.FirstOrDefault(z => z.Name == obs.ZoneName).RectItems.Add(vm);

            }

            var vmTemplates = templates.Select(t => new DeviceTemplateViewModel(t)).ToList();


            return new ImportResult { Templates = vmTemplates, Zones = zones };
        }

        public List<ObstacleDto> ImportObstaclesFromExcel(string filePath)
        {
            List<ObstacleDto> obstacles = new List<ObstacleDto>();

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                XSSFWorkbook workbook = new XSSFWorkbook(fileStream);
                ISheet sheet = workbook.GetSheet("Obstacles"); // Предполагаем, что данные о препятствиях находятся на первом листе

                int rowCount = sheet.PhysicalNumberOfRows;
                for (int i = 1; i < rowCount; i++) // Начинаем с 1, чтобы пропустить заголовки столбцов
                {
                    IRow row = sheet.GetRow(i);
                    if (row != null)
                    {
                        ObstacleDto obstacle = new ObstacleDto();
                        obstacle.Name = row.GetCell(0).StringCellValue;
                        obstacle.X = (int)row.GetCell(1).NumericCellValue;
                        obstacle.Y = (int)row.GetCell(2).NumericCellValue;
                        obstacle.Width = (int)row.GetCell(3).NumericCellValue;
                        obstacle.Height = (int)row.GetCell(4).NumericCellValue;
                        obstacle.ZoneName = row.GetCell(5).StringCellValue;                        

                        obstacles.Add(obstacle);
                    }
                }
            }

            return obstacles;
        }


        public List<ProvZoneViewModel> ImportZonesFromExcel(string filePath)
        {
            List<ProvZoneViewModel> zones = new List<ProvZoneViewModel>();

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                XSSFWorkbook workbook = new XSSFWorkbook(fileStream);
                ISheet sheet = workbook.GetSheet("Zone"); // Предполагаем, что данные о зонах находятся на первом листе

                int rowCount = sheet.PhysicalNumberOfRows;
                for (int i = 1; i < rowCount; i++) // Начинаем с 1, чтобы пропустить заголовки столбцов
                {
                    IRow row = sheet.GetRow(i);
                    if (row != null)
                    {
                        ProvZoneViewModel zone = new ProvZoneViewModel();
                        zone.Name = row.GetCell(0).StringCellValue;
                        zone.Width = (int)row.GetCell(1).NumericCellValue;
                        zone.Height = (int)row.GetCell(2).NumericCellValue;
                        zones.Add(zone);
                    }
                }
            }

            return zones;
        }

        public List<DeviceTemplate> ImportDeviceTemplatesFromExcel(string filePath)
        {
            List<DeviceTemplate> deviceTemplates = new List<DeviceTemplate>();

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                XSSFWorkbook workbook = new XSSFWorkbook(fileStream);
                ISheet sheet = workbook.GetSheet("Device Templates"); // Предполагаем, что данные DeviceTemplate находятся на первом листе

                int rowCount = sheet.PhysicalNumberOfRows;
                for (int i = 1; i < rowCount; i++) // Начинаем с 1, чтобы пропустить заголовки столбцов
                {
                    IRow row = sheet.GetRow(i);
                    if (row != null)
                    {
                        DeviceTemplate deviceTemplate = new DeviceTemplate();
                        deviceTemplate.Name = row.GetCell(0).StringCellValue;
                        deviceTemplate.Width = (int)row.GetCell(1).NumericCellValue;
                        deviceTemplate.Height = (int)row.GetCell(2).NumericCellValue;
                        deviceTemplate.Count = (int)row.GetCell(3).NumericCellValue;

                        Area workArea = new Area();
                        workArea.Width = (int)row.GetCell(4).NumericCellValue;
                        workArea.Height = (int)row.GetCell(5).NumericCellValue;
                        workArea.AreaType = (AreaType)Enum.Parse(typeof(AreaType), row.GetCell(6).StringCellValue);
                        workArea.X = (int)row.GetCell(7).NumericCellValue;
                        workArea.Y = (int)row.GetCell(8).NumericCellValue;
                        deviceTemplate.WorkArea = workArea;

                        Area serviceArea = new Area();
                        serviceArea.Width = (int)row.GetCell(9).NumericCellValue;
                        serviceArea.Height = (int)row.GetCell(10).NumericCellValue;
                        serviceArea.AreaType = (AreaType)Enum.Parse(typeof(AreaType), row.GetCell(11).StringCellValue);
                        serviceArea.X = (int)row.GetCell(12).NumericCellValue;
                        serviceArea.Y = (int)row.GetCell(13).NumericCellValue;
                        deviceTemplate.ServiceArea = serviceArea;

                        deviceTemplates.Add(deviceTemplate);
                    }
                }
            }

            return deviceTemplates;
        }


        public List<DeviceDto> ImportDevicesFromExcel(string filePath)
        {
            var devices = new List<DeviceDto>();
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                XSSFWorkbook workbook = new XSSFWorkbook(fileStream);
                ISheet sheet = workbook.GetSheet("Devices"); // Предполагаем, что данные устройств находятся на первом листе

                int rowCount = sheet.PhysicalNumberOfRows;
                for (int i = 1; i < rowCount; i++) // Начинаем с 1, чтобы пропустить заголовки столбцов
                {
                    IRow row = sheet.GetRow(i);
                    if (row != null)
                    {
                        var device = new DeviceDto();
                        device.ZoneName = row.GetCell(14).StringCellValue;
                        device.TemplateType = row.GetCell(13).StringCellValue;
                        device.X = (int)row.GetCell(3).NumericCellValue;
                        device.Y = (int)row.GetCell(4).NumericCellValue;
                        devices.Add(device);
                    }
                }
            }

            return devices;
        }
    }
}
