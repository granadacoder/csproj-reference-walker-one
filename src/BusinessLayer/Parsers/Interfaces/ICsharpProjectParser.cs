using System.Text;

using MyCompany.MyExamples.ProjectParser.Domain.Dtos;

namespace MyCompany.MyExamples.ProjectParser.BusinessLayer.Parsers.Interfaces
{
    public interface ICsharpProjectParser
    {
        CsParseResult Parse(string fileName, int nestLevel);

        string ConvertCsParseResult(CsParseResult input, StringBuilder sb, int nestLevel);
    }
}
