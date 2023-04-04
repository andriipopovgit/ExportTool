using CsvHelper;
using Microsoft.Win32;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.Utility;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;

namespace ExportTool
{
    public static class ContextMenuCommands
    {
        public static BaseCommand? toCsv = null;
        public static BaseCommand? toTxt = null;
        public static string? format = null;

        public static BaseCommand ToCsv
        {
            get
            {
                format = "CSV files (*.csv)|*.csv";
                if (toCsv == null)
                    toCsv = new BaseCommand(OnExportClicked);
                return toCsv;
            }
        }
        public static BaseCommand ToTxt
        {
            get
            {
                format = "TXT files (*.txt)|*.txt";
                if (toTxt == null)
                    toTxt = new BaseCommand(OnExportClicked);
                return toTxt;
            }
        }

        public static void OnExportClicked(object obj)
        {
            var grid = (obj as GridRecordContextMenuInfo)?.DataGrid;
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            var items = new ObservableCollection<OrderInfo?>();
            if (grid != null)
                foreach (var item in grid.SelectedItems)
                    items.Add(item as OrderInfo);

            saveFileDialog.Filter = format;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == true)
            {
                using (var writer = new StreamWriter(saveFileDialog.OpenFile()))
                {
                    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    {
                        csv.WriteRecords(items);
                    }
                }
            }
        }
    }
}
