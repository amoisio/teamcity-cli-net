using System.Linq;
using System.Threading.Tasks;
using CliFx;

namespace TeamCityCliNet.Commands
{
    public abstract class FieldsCommand<T> : ICommand
    {
        public ValueTask ExecuteAsync(IConsole console)
        {
            var props = Utilities.AllPrintablePropertiesOf<T>();
            var names = props.Select(prop => prop.Name).OrderBy(e => e);
            foreach(var name in names)
                console.Output.WriteLine(name);

            return default;
        }
    }
}