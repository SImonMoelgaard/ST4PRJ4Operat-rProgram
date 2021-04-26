using System;
using System.Collections.Generic;
using System.Text;
using OperatoerLibrary.DTO;

namespace OperatoerLibrary.Filters
{
    public interface IGatingArea
    {
        public string SaveGatingArea(double lowerGating, double higherGating);
        public DTO_GatingValues GetGatingValue();
    }
}
