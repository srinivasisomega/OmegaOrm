using OmegaOrm;
using OmegaOrm.Models;
class Program
{
    static void Main()
    {
        var connectionString = "Server=COGNINE-L105;Database=bb2;Trusted_Connection=True;Trust Server Certificate=True;";

        // User-defined models
        var models = new List<Type>
        {
            typeof(User),
            typeof(Address),
            typeof(Role)
        };

        // Synchronize database schema
        SchemaSynchronizer.SynchronizeDatabase(connectionString, models);
    }
}
