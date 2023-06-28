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

namespace EquipmentLayout.Models
{
    public class ExcelExporter
    {

        public void ExportToExel(List<ProvZoneViewModel> zones, List<DeviceTemplate> templates, string filePath)
        {
            var workbook = new XSSFWorkbook();
            ExportZoneToExcel(zones, filePath, workbook);
            ExportObstaclesToExcel(zones, filePath, workbook);
            ExportTemplatesToExcel(templates, filePath, workbook);
            ExportDevicesToExcel(zones, filePath, workbook);
        }

        public void ExportZoneToExcel(List<ProvZoneViewModel> zones, string filePath, IWorkbook workbook = null)
        {
            if (workbook == null)
                workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Zone");

            // Создание заголовков столбцов
            IRow headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("X");
            headerRow.CreateCell(1).SetCellValue("Y");
            headerRow.CreateCell(2).SetCellValue("Width");
            headerRow.CreateCell(3).SetCellValue("Height");
            headerRow.CreateCell(3).SetCellValue("Name");

            // Заполнение данными для каждого объекта Obstacle
            for (int i = 0; i < zones.Count; i++)
            {
                ProvZoneViewModel zone = zones[i];
                IRow dataRow = sheet.CreateRow(i + 1);
                dataRow.CreateCell(1).SetCellValue(zone.Name);
                dataRow.CreateCell(0).SetCellValue(zone.Width);
                dataRow.CreateCell(1).SetCellValue(zone.Height);
                dataRow.CreateCell(1).SetCellValue(zone.Name);

            }

            // Сохранение рабочей книги в файл
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fileStream);
            }
        }

        public void ExportObstaclesToExcel(List<ProvZoneViewModel> zones, string filePath, IWorkbook workbook = null)
        {
            if (workbook == null)
                workbook = new XSSFWorkbook(); ISheet sheet = workbook.CreateSheet("Obstacles");

            var obstacles = zones.SelectMany(z => z.ObstacleViewModels.Select(o => o.Model)).ToList();

            // Создание заголовков столбцов
            IRow headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("X");
            headerRow.CreateCell(1).SetCellValue("Y");
            headerRow.CreateCell(2).SetCellValue("Width");
            headerRow.CreateCell(3).SetCellValue("Height");
            headerRow.CreateCell(3).SetCellValue("Zone Name");

            // Заполнение данными для каждого объекта Obstacle
            for (int i = 0; i < obstacles.Count; i++)
            {
                Obstacle obstacle = obstacles[i];

                var zoneName = zones.FirstOrDefault(z => z.ObstacleViewModels.Select(vm => vm.Model).Contains(obstacle)).Name;

                IRow dataRow = sheet.CreateRow(i + 1);
                dataRow.CreateCell(0).SetCellValue(obstacle.Name);
                dataRow.CreateCell(0).SetCellValue(obstacle.X);
                dataRow.CreateCell(1).SetCellValue(obstacle.Y);
                dataRow.CreateCell(2).SetCellValue(obstacle.Width);
                dataRow.CreateCell(3).SetCellValue(obstacle.Height);
                dataRow.CreateCell(4).SetCellValue(zoneName);
            }

            // Сохранение рабочей книги в файл
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fileStream);
            }
        }

        public void ExportTemplatesToExcel(List<DeviceTemplate> deviceTemplates, string filePath, IWorkbook workbook = null)
        {
            if (workbook == null)
                workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Device Templates");

            // Создание заголовков столбцов
            IRow headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("Width");
            headerRow.CreateCell(1).SetCellValue("Height");
            headerRow.CreateCell(2).SetCellValue("Name");
            headerRow.CreateCell(3).SetCellValue("Count");
            headerRow.CreateCell(4).SetCellValue("Work Area Width");
            headerRow.CreateCell(5).SetCellValue("Work Area Height");
            headerRow.CreateCell(6).SetCellValue("Work Area Type");
            headerRow.CreateCell(7).SetCellValue("Work Area X");
            headerRow.CreateCell(8).SetCellValue("Work Area Y");
            headerRow.CreateCell(9).SetCellValue("Service Area Width");
            headerRow.CreateCell(10).SetCellValue("Service Area Height");
            headerRow.CreateCell(11).SetCellValue("Service Area Type");
            headerRow.CreateCell(12).SetCellValue("Service Area X");
            headerRow.CreateCell(13).SetCellValue("Service Area Y");

            // Заполнение данными для каждого объекта DeviceTemplate
            for (int i = 0; i < deviceTemplates.Count; i++)
            {
                DeviceTemplate template = deviceTemplates[i];
                IRow dataRow = sheet.CreateRow(i + 1);
                dataRow.CreateCell(0).SetCellValue(template.Width);
                dataRow.CreateCell(1).SetCellValue(template.Height);
                dataRow.CreateCell(2).SetCellValue(template.Name);
                dataRow.CreateCell(3).SetCellValue(template.Count);
                dataRow.CreateCell(4).SetCellValue(template.WorkArea.Width);
                dataRow.CreateCell(5).SetCellValue(template.WorkArea.Height);
                dataRow.CreateCell(6).SetCellValue(template.WorkArea.AreaType.ToString());
                dataRow.CreateCell(7).SetCellValue(template.WorkArea.X);
                dataRow.CreateCell(8).SetCellValue(template.WorkArea.Y);
                dataRow.CreateCell(9).SetCellValue(template.ServiceArea.Width);
                dataRow.CreateCell(10).SetCellValue(template.ServiceArea.Height);
                dataRow.CreateCell(11).SetCellValue(template.ServiceArea.AreaType.ToString());
                dataRow.CreateCell(12).SetCellValue(template.ServiceArea.X);
                dataRow.CreateCell(13).SetCellValue(template.ServiceArea.Y);
            }

            // Сохранение рабочей книги в файл
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fileStream);
            }
        }

        public void ExportDevicesToExcel(List<ProvZoneViewModel> zones, string filePath, IWorkbook workbook = null)
        {
            // Создание нового документа Excel
            if (workbook == null)
                workbook = new XSSFWorkbook();

            var devices = zones.SelectMany(z => z.DeviceViewModels.Select(o => o.Model)).ToList();

            ISheet sheet = workbook.CreateSheet("Devices");

            // Создание заголовков столбцов
            int rowIndex = 0;
            IRow headerRow = sheet.CreateRow(rowIndex++);
            headerRow.CreateCell(0).SetCellValue("Name");
            headerRow.CreateCell(1).SetCellValue("Width");
            headerRow.CreateCell(2).SetCellValue("Height");
            headerRow.CreateCell(3).SetCellValue("X");
            headerRow.CreateCell(4).SetCellValue("Y");
            headerRow.CreateCell(5).SetCellValue("WorkArea Width");
            headerRow.CreateCell(6).SetCellValue("WorkArea Height");
            headerRow.CreateCell(7).SetCellValue("WorkArea X");
            headerRow.CreateCell(8).SetCellValue("WorkArea Y");
            headerRow.CreateCell(9).SetCellValue("ServiceArea Width");
            headerRow.CreateCell(10).SetCellValue("ServiceArea Height");
            headerRow.CreateCell(11).SetCellValue("ServiceArea X");
            headerRow.CreateCell(12).SetCellValue("ServiceArea Y");
            headerRow.CreateCell(13).SetCellValue("Template Type");
            headerRow.CreateCell(14).SetCellValue("Zone Name");

            // Заполнение данными из объектов Device
            foreach (var device in devices)
            {
                IRow dataRow = sheet.CreateRow(rowIndex++);

                var zoneName = zones.FirstOrDefault(z => z.DeviceViewModels.Select(vm=>vm.Model).Contains(device)).Name;
                
                dataRow.CreateCell(0).SetCellValue(device.Name);
                dataRow.CreateCell(1).SetCellValue(device.Width);
                dataRow.CreateCell(2).SetCellValue(device.Height);
                dataRow.CreateCell(3).SetCellValue(device.X);
                dataRow.CreateCell(4).SetCellValue(device.Y);
                dataRow.CreateCell(5).SetCellValue(device.WorkArea.Width);
                dataRow.CreateCell(6).SetCellValue(device.WorkArea.Height);
                dataRow.CreateCell(7).SetCellValue(device.WorkArea.X);
                dataRow.CreateCell(8).SetCellValue(device.WorkArea.Y);
                dataRow.CreateCell(9).SetCellValue(device.ServiceArea.Width);
                dataRow.CreateCell(10).SetCellValue(device.ServiceArea.Height);
                dataRow.CreateCell(11).SetCellValue(device.ServiceArea.X);
                dataRow.CreateCell(12).SetCellValue(device.ServiceArea.Y);
                dataRow.CreateCell(13).SetCellValue(device.Template.Name);
                dataRow.CreateCell(14).SetCellValue(zoneName);
            }

            // Автоматическое изменение ширины столбцов
            for (int i = 0; i < 15; i++)
            {
                sheet.AutoSizeColumn(i);
            }

            // Сохранение документа Excel в файл
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fileStream);
            }
        }


    }
}
