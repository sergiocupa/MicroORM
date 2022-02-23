

using System.Text;


namespace MicroORM.Builder
{
    public interface CommandBuilder
    {

        void SelectBuild(string header, StringBuilder stringb);

        void WhereBuild(string header, StringBuilder stringb);


    }
}
