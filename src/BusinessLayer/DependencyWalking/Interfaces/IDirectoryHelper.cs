namespace MyCompany.MyExamples.ProjectParser.BusinessLayer.DependencyWalking.Interfaces
{
    public interface IDirectoryHelper
    {
        string GetRootDirectory();

        string GetRootDirectoryAndCombine(string suffixPath);
    }
}