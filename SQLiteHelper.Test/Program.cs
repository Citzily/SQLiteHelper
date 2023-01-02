using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SQLiteHelper;

namespace SQLiteHelper.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            double[] insertValues = GetTXTDatas("D:\\[Project]\\揚鴻\\40000.txt");




            string dbPath = $"{Directory.GetCurrentDirectory()}\\Test.db";

            CreateDB(dbPath, "Valtage300", "Valtage350", "Valtage400");


        }


        static double[] GetTXTDatas(string path)
        {
            List<double> results = new List<double>();
            using (StreamReader fs = new StreamReader(path))
            {
                string value = fs.ReadLine();
                while (!string.IsNullOrEmpty(value))
                {
                    if (double.TryParse(value, out double parseValue))
                        results.Add(parseValue);
                    value = fs.ReadLine();
                }
            }
            return results.ToArray();
        }

        private static SQLiteDatabase CreateDB(string dbPath, params string[] tableNamea)
        {
            SQLiteDatabase db = new SQLiteDatabase(dbPath);

            foreach (var name in tableNamea)
            {
                SQLiteTable Valtage300 = new SQLiteTable(db, name,
                new SQLiteRealColumn("MJ", 0),
                new SQLiteIntColumn("Time", 0),
                new SQLiteIntColumn("Shot", 0),
                new SQLiteRealColumn("Pick1", 0),
                new SQLiteRealColumn("Pick2", 0),
                new SQLiteRealColumn("Pick3", 0),
                new SQLiteRealColumn("Pick4", 0),
                new SQLiteRealColumn("Pick5", 0),
                new SQLiteRealColumn("Pick6", 0),
                new SQLiteRealColumn("Pick7", 0),
                new SQLiteRealColumn("Pick8", 0),
                new SQLiteRealColumn("Pick9", 0),
                new SQLiteRealColumn("Pick10", 0),
                new SQLiteRealColumn("Pick11", 0),
                new SQLiteRealColumn("Pick12", 0),
                new SQLiteRealColumn("Pick13", 0),
                new SQLiteRealColumn("Pick14", 0),
                new SQLiteRealColumn("Pick15", 0),
                new SQLiteRealColumn("Pick16", 0),
                new SQLiteRealColumn("Pick17", 0),
                new SQLiteRealColumn("Pick18", 0),
                new SQLiteRealColumn("Pick19", 0),
                new SQLiteRealColumn("Pick20", 0),
                new SQLiteRealColumn("Pick21", 0),
                new SQLiteRealColumn("Pick22", 0),
                new SQLiteRealColumn("Pick23", 0),
                new SQLiteRealColumn("Pick24", 0),
                new SQLiteRealColumn("Pick25", 0),
                new SQLiteRealColumn("Pick26", 0),
                new SQLiteRealColumn("Pick27", 0),
                new SQLiteRealColumn("Pick28", 0),
                new SQLiteRealColumn("Pick29", 0),
                new SQLiteRealColumn("Pick30", 0),
                new SQLiteRealColumn("Pick31", 0),
                new SQLiteRealColumn("Pick32", 0),
                new SQLiteRealColumn("Pick33", 0),
                new SQLiteRealColumn("Pick34", 0),
                new SQLiteRealColumn("Pick35", 0),
                new SQLiteRealColumn("Pick36", 0),
                new SQLiteRealColumn("Pick37", 0),
                new SQLiteRealColumn("Pick38", 0),
                new SQLiteRealColumn("Pick39", 0),
                new SQLiteRealColumn("Pick40", 0));
            }

            return db;
        }

    }
}
