using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nile.Data.IO
{
    public class FileProductDatabase : ProductDatabase
    {
        public FileProductDatabase ( string filename )
        {
            _filename = filename;
        }
        protected override Product AddCore( Product product )
        {
            EnsureInitialized();

            product.Id = _id++;
            _items.Add(product);

            SaveData();

            return product;
        }

        protected override IEnumerable<Product> GetAllCore()
        {
            EnsureInitialized();

            //LoadData();
            return _items;
        }

        //Ensure file is loaded
        private void EnsureInitialized ()
        {
            if (_items == null)
            {
                _items = LoadData();

                //_id = 0;
                //foreach (var item in _items)
                //{
                //    if (item.Id > _id)
                //        _id = item.Id;
                //};
                if (_items.Any())
                {
                    _id = _items.Max(i => i.Id);
                    ++_id;
                };
            };
        }

        private List<Product> LoadData()
        {
            var items = new List<Product>();

            try
            {
                //Make sure the file exists
                if (!File.Exists(_filename))
                    return items;

                var lines = File.ReadAllLines(_filename);
                foreach (var line in lines)
                {
                    var fields = line.Split(',');

                    var product = new Product() {
                        Id = ParseInt32(fields[0]),
                        Name = fields[1],
                        Description = fields[2],
                        Price = ParseDecimal(fields[3]),
                        IsDiscontinued = ParseInt32(fields[4]) != 0
                    };
                    items.Add(product);
                };

                return items;
            } catch (Exception e)
            {
                throw new Exception("Failure loading data", e);
            };
        }

        private void SaveData()
        {
            using (var stream = File.OpenWrite(_filename))
            using (var writer = new StreamWriter(stream))
            {
                foreach (var item in _items)
                {
                    var line = $"{item.Id},{item.Name},{item.Description}," +
                               $"{item.Price},{(item.IsDiscontinued ? 1 : 0)}";

                    writer.WriteLine(line);
                };
            };
        }

        private void SaveDataPoorer()
        {
            Stream stream = null;
            StreamWriter writer = null;
            try
            {
                stream = File.OpenWrite(_filename);
                writer = new StreamWriter(stream);

                foreach (var item in _items)
                {
                    var line = $"{item.Id},{item.Name},{item.Description},{item.Price},{(item.IsDiscontinued ? 1 : 0)}";

                    writer.WriteLine(line);
                };
            } catch (ArgumentException e)
            {
                //Rethrowing exception
                throw;
                //throw e;
                //Never right!!!
            } catch (Exception e)
            {
                //Example of wrapping an exception to hide details
                throw new Exception("Save failed", e);
            } finally
            {
                writer?.Close();
                stream?.Close();
            };
        }

        private void SaveDataNonStream ()
        {
            var lines =_items.Select(item =>
                            $"{item.Id},{item.Name},{item.Description}," +
                            $"{item.Price},{(item.IsDiscontinued ? 1 : 0)}");

            //var lines = new List<string>();
            
            //foreach (var item in _items)
            //{
            //    var line = $"{item.Id},{item.Name},{item.Description},{item.Price},{(item.IsDiscontinued ? 1 : 0)}";
            //    lines.Add(line);
            //};

            File.WriteAllLines(_filename, lines);
        }

        private int ParseInt32 ( string value )
        {
            if (Int32.TryParse(value, out var result))
                return result;

            return -1;
        }

        private decimal ParseDecimal( string value )
        {
            if (Decimal.TryParse(value, out var result))
                return result;

            return -1;
        }
        protected override Product GetCore( int id )
        {
            EnsureInitialized();

            return _items.FirstOrDefault(i => i.Id == id);

            //foreach (var item in _items)
            //{
            //    if (item.Id == id)
            //        return item;
            //};

            //return null;
        }

        //private bool IsId ( Product product )
        //{
        //    return product.Id == id;
        //}

        protected override Product GetProductByNameCore( string name )
        {
            EnsureInitialized();

            return _items.FirstOrDefault(
                    i => String.Compare(i.Name, name, true) == 0);

            //foreach (var item in _items)
            //{
            //    if (String.Compare(item.Name, name, true) == 0)
            //        return item;
            //};

            //return null;
        }

        protected override void RemoveCore( int id )
        {
            EnsureInitialized();

            var product = GetCore(id);
            if (product != null)
            {
                _items.Remove(product);
                SaveData();
            };
        }

        protected override Product UpdateCore( Product product )
        {
            EnsureInitialized();

            var existing = GetCore(product.Id);

            _items.Remove(existing);
            _items.Add(product);

            SaveData();

            return product;
        }

        private readonly string _filename;
        private List<Product> _items;
        private int _id;
    }
}
