using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvestmentSimulator.DBFiles
{
    public class StockProfile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AssetProfileId { get; set; }

        /// AssetId
        public long AssetFK { get; set; }

        /// Date of entry
        public DateTime Date { get; set; }

        /// Market Capitalisation
        public decimal MarketCap { get; set; }

        /// Number of shares outstanding
        public decimal ShareOutstanding { get; set; }

        /// Number of employees
        public int EmployeeTotal { get; set; }

        /// Address of company's headquarter.
        [MaxLength(250)]
        public string Address { get; set; }

        /// City of company's headquarter.
        [MaxLength(50)]
        public string City { get; set; }

        /// State of company's headquarter.
        [MaxLength(50)]
        public string State { get; set; }

        /// Country of company's headquarter.
        [MaxLength(50)]
        public string Country { get; set; }

        /// Company business summary.
        [MaxLength(250)]
        public string Description { get; set; }

        /// Company name.
        [MaxLength(50)]
        public string Name { get; set; }

        /// Company phone number.
        [MaxLength(15)]
        public string Phone { get; set; }

        /// Company website.
        [MaxLength(100)]
        public string WebURL { get; set; }

        /// GICS industry group.
        [MaxLength(250)]
        public string GGroup { get; set; }

        /// GICS industry.
        [MaxLength(250)]
        public string GInd { get; set; }

        /// GICS sector.
        [MaxLength(250)]
        public string GSector { get; set; }

        /// GICS sub-industry.
        [MaxLength(250)]
        public string GSubInd { get; set; }

        /// NAICS national industry.
        [MaxLength(250)]
        public string NNatInd { get; set; }

        /// NAICS industry.
        [MaxLength(250)]
        public string NInd { get; set; }

        /// NAICS sector.
        [MaxLength(250)]
        public string NSector { get; set; }

        /// NAICS subsector.
        [MaxLength(250)]
        public string NSubsector { get; set; }
    }
}
