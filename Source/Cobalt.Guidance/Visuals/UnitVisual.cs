using Cobalt.Core;

namespace Cobalt.Guidance.Visuals
{
    public class UnitVisual : Visual<Unit>
    {
        private string unitTemplate = @"
<svg width=""100%"" height=""100%"" viewBox=""0 0 2363 3340"" version=""1.1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" xml:space=""preserve"" xmlns:serif=""http://www.serif.com/"" style=""fill-rule:evenodd;clip-rule:evenodd;stroke-linecap:round;stroke-linejoin:round;stroke-miterlimit:1.5;"">
    <g id=""Slider"" transform=""matrix(3.48012,0,0,2.08333,-125.046,245.59)"">
        <rect x=""48"" y=""28"" width=""654"" height=""76"" style=""fill:none;""/>
        <clipPath id=""_clip1"">
            <rect x=""48"" y=""28"" width=""654"" height=""76""/>
        </clipPath>
        <g clip-path=""url(#_clip1)"">
            <g transform=""matrix(1.5,2.24706e-18,3.33995e-18,1,-1640,-2217.42)"">
                <path d=""M1561.34,2282.38C1561.34,2281.85 1561.16,2281.41 1560.95,2281.41L1125.72,2281.41C1125.51,2281.41 1125.34,2281.85 1125.34,2282.38L1125.34,2284.45C1125.34,2284.99 1125.51,2285.41 1125.72,2285.41L1560.95,2285.41C1561.16,2285.41 1561.34,2284.99 1561.34,2284.45L1561.34,2282.38Z"" style=""fill:rgb(182,182,182);""/>
            </g>
            <g transform=""matrix(0.287346,0,0,0.48,34.2074,34.32)"">
                <rect x=""48"" y=""28"" width=""240"" height=""76"" style=""fill:none;""/>
                <g transform=""matrix(0.472477,5.07592e-19,4.65724e-18,1,-483.696,-2217.42)"">
                    <path d=""M1561.34,2283.41C1561.34,2282.31 1559.44,2281.41 1557.1,2281.41L1129.57,2281.41C1127.23,2281.41 1125.34,2282.31 1125.34,2283.41C1125.34,2284.52 1127.23,2285.41 1129.57,2285.41L1557.1,2285.41C1559.44,2285.41 1561.34,2284.52 1561.34,2283.41Z"" style=""fill:rgb(0,122,255);""/>
                </g>
                <g transform=""matrix(0.983607,0,0,0.95,-5638.23,-2138)"">
                    <rect x=""5781"" y=""2280"" width=""244"" height=""80"" style=""fill:none;""/>
                </g>
            </g>
        </g>
    </g>
    <g id=""Folder"" transform=""matrix(2.08333,0,0,2.08333,-16043.7,-1110.74)"">
        <g transform=""matrix(0.6,0,0,0.6,4452.12,161.2)"">
            <circle cx=""7099"" cy=""798"" r=""80"" style=""fill:rgb(62,111,169);""/>
        </g>
        <g transform=""matrix(2,0,0,2,8663.5,592)"">
            <path d=""M13.011,19L35.011,19L35.011,32.008C35.011,33.108 34.121,34 33.019,34L15.002,34C13.902,34 13.011,33.107 13.011,32.008L13.011,19ZM20.011,22.5C20.011,23.056 20.457,23.5 21.008,23.5L27.014,23.5C27.558,23.5 28.011,23.052 28.011,22.5C28.011,21.944 27.564,21.5 27.014,21.5L21.008,21.5C20.464,21.5 20.011,21.948 20.011,22.5ZM12.907,14.888C13.163,14.397 13.816,14 14.369,14L33.654,14C34.205,14 34.86,14.398 35.116,14.888L35.82,16.234C36.329,17.209 35.851,18 34.745,18L13.278,18C12.175,18 11.694,17.21 12.204,16.234L12.907,14.888Z"" style=""fill:white;""/>
        </g>
    </g>
    <g transform=""matrix(2.13508,0,0,3.84695,-1821.93,-5593.77)"">
        <rect x=""873"" y=""1465"" width=""1066"" height=""845"" style=""fill:white;fill-opacity:0;stroke:rgb(0,13,49);stroke-width:6.03px;""/>
    </g>
    <text x=""92px"" y=""569.703px"" style=""font-family:'ArialMT', 'Arial', sans-serif;font-size:150px;"">Property 1           value</text>
    <g transform=""matrix(1,0,0,1,-26,-911.744)"">
        <text x=""118px"" y=""1170.69px"" style=""font-family:'ArialMT', 'Arial', sans-serif;font-size:125px;"">Bill - Dec - Power</text>
    </g>
</svg>
";

        public override string Compile()
        {
            return unitTemplate;
        }

     
    }
}