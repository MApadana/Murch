using System.Collections.Generic;


namespace Exon.Recab.Service.Model.ProdoctModel
{
    public class AggregateResultModel
    {
        //rename : aggregates 
        public List<CFAggregateViewModel> aggregates { get; set; }

        //must do it : totalCount
        public long totalCount { get; set; }

        //rename : selectedItems  
        public List<SelectItemFilterSearchModel> selectedItems { get; set; }

        public AggregateResultModel()
        {
            aggregates = new List<CFAggregateViewModel>();

            selectedItems = new List<SelectItemFilterSearchModel>();
        }
    }
}
