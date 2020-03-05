
using System;

namespace TES3.Records.SubRecords
{
    public class TES3HeaderData : SubRecord
    {
        public float Version { get; set; }
        public int FileType { get; set; }
        string companyName;
        string description;
        public int NumRecords { get; set; }

        public TES3HeaderData(string name, float version, int fileType, string companyName, string description,
                int numRecords) : base(name)
        {
            Version = version;
            FileType = fileType;
            CompanyName = companyName;
            Description = description;
            NumRecords = numRecords;
        }

        public string CompanyName
        {
            get => companyName;
            set => companyName = value ?? throw new ArgumentNullException("companyName", "Company Name cannot be null.");
        }

        public string Description
        {
            get => description;
            set => description = value ?? throw new ArgumentNullException("description", "Description cannot be null.");
        }


        public override string ToString()
        {
            return $"{Name} [Version: {Version}, Type: {FileType}, Company: {CompanyName}, Description: {Description}, Record Count: {NumRecords}]";
        }
    }


}