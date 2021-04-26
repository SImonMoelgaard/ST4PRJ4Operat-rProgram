using System;
using System.Collections.Generic;
using System.Text;
using OperatoerLibrary.DTO;

namespace OperatoerLibrary.Filters
{
    public class GatingArea
    {
        private DTO_GatingValues gatingValue;
        public string SaveGatingArea(double lowerGating, double higherGating)
        {
            
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
            else
            {
                gatingValue = new DTO_GatingValues(higherGating, lowerGating);
                return "Success";
                
            }
        }

        public DTO_GatingValues GetGatingValue()
        {
            return gatingValue;
        }


    }
}
