namespace MyCompany.MyExamples.ProjectParser.Domain.Entities
{
    using System.Collections.Generic;

    /// <summary>
    /// Navigation and Other properties
    /// </summary>
    public partial class MyParentEntity
    {
        public MyParentEntity()
        {
            this.MyChilds = new List<MyChildEntity>();
        }

        public ICollection<MyChildEntity> MyChilds { get; set; }
    }
}
