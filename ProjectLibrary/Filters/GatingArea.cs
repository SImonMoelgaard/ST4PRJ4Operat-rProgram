using OperatoerLibrary.DTO;

namespace OperatoerLibrary.Filters
{
    public class GatingArea : IGatingArea
    {
        public DTO_GatingValues gatingValue { get; set; }
        
        public string SaveGatingArea(double lowerGating, double higherGating)
        {
            gatingValue = new DTO_GatingValues(0, 0);
            if (lowerGating > higherGating)
            {
                return "Lower value cannot be higher than higher value";
            }
            else if (lowerGating<0)
            {
                return "Value cannot be 0";
            }
            else if (higherGating<0)
            {
                return "Value cannot be 0";
            }
            else if (higherGating>lowerGating)
            {
                gatingValue = new DTO_GatingValues(higherGating, lowerGating);
                return "Success";

            }

            return "none";
        }

        public DTO_GatingValues GetGatingValue()
        {
            return gatingValue;
        }


    }
}
