using System.Collections.Generic;

namespace Nile.Data
{
    public interface IProductDatabase
    {
        Product Add( Product product );
        Product Update( Product product );
        IEnumerable<Product> GetAll();
        void Remove( int id );
    }
}