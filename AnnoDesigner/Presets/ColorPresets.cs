using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AnnoDesigner.Presets
{
    [DataContract]
    public class ColorPresets
    {
        public ColorPresets()
        {
            Version = string.Empty;
            AvailableSchemes = new List<ColorScheme>();
        }

        [DataMember(Order = 0)]
        public string Version { get; set; }

        [DataMember(Order = 1, Name = "AvailableSchemes")]
        public List<ColorScheme> AvailableSchemes { get; set; }
    }
}