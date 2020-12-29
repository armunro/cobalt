using Cobalt.Guidance.Descriptions;

namespace Cobalt.Guidance.Visuals
{
    public class StageVisual : Visual<Stage.Stage>
    {


        private StageDescription _stageDescription;


        public override string Compile()
        {
            return  @"
<svg width=""100%"" height=""100%"" viewBox=""0 0 200 100"" version=""1.1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" xml:space=""preserve"" xmlns:serif=""http://www.serif.com/"" style=""fill-rule:evenodd;clip-rule:evenodd;stroke-linecap:round;stroke-linejoin:round;stroke-miterlimit:1.5;"">
    <rect x=""2"" y=""2"" width=""195"" height=""96"" style=""fill:rgb(0,52,77);stroke:rgb(132,132,132);stroke-width:1px;""/>
    <g transform=""matrix(1,0,0,1,-18,4)"">
        <text x=""24px"" y=""16.923px"" style=""font-family:'ArialMT', 'Arial', sans-serif;font-size:20.833px;fill:white;"">Name</text>
    </g>
    <g transform=""matrix(0.793814,0,0,0.797644,0.237113,21.8191)"">
        <path d=""M103,11.22C103,8.339 100.65,6 97.755,6L11.245,6C8.35,6 6,8.339 6,11.22L6,87.78C6,90.661 8.35,93 11.245,93L97.755,93C100.65,93 103,90.661 103,87.78L103,11.22Z"" style=""fill:white;stroke:rgb(132,132,132);stroke-width:1.26px;""/>
    </g>
    <g transform=""matrix(0.793814,0,0,0.797644,112.237,21.8191)"">
        <path d=""M103,11.22C103,8.339 100.65,6 97.755,6L11.245,6C8.35,6 6,8.339 6,11.22L6,87.78C6,90.661 8.35,93 11.245,93L97.755,93C100.65,93 103,90.661 103,87.78L103,11.22Z"" style=""fill:white;stroke:rgb(132,132,132);stroke-width:1.26px;""/>
    </g>
    <g id=""Folder"" transform=""matrix(0.604167,0,0,0.604167,-5219.71,-325.364)"">
        <g transform=""matrix(0.6,0,0,0.6,4452.12,161.2)"">
            <circle cx=""7099"" cy=""798"" r=""80"" style=""fill:rgb(62,111,169);""/>
        </g>
        <g transform=""matrix(2,0,0,2,8663.5,592)"">
            <path d=""M13.011,19L35.011,19L35.011,32.008C35.011,33.108 34.121,34 33.019,34L15.002,34C13.902,34 13.011,33.107 13.011,32.008L13.011,19ZM20.011,22.5C20.011,23.056 20.457,23.5 21.008,23.5L27.014,23.5C27.558,23.5 28.011,23.052 28.011,22.5C28.011,21.944 27.564,21.5 27.014,21.5L21.008,21.5C20.464,21.5 20.011,21.948 20.011,22.5ZM12.907,14.888C13.163,14.397 13.816,14 14.369,14L33.654,14C34.205,14 34.86,14.398 35.116,14.888L35.82,16.234C36.329,17.209 35.851,18 34.745,18L13.278,18C12.175,18 11.694,17.21 12.204,16.234L12.907,14.888Z"" style=""fill:white;""/>
        </g>
    </g>
    <g id=""Forward--small-"" serif:id=""Forward (small)"" transform=""matrix(1,3.69779e-32,0,1,-198.6,-158.5)"">
        <path d=""M311.2,217.5L300,206.5L300,213L286,213L286,222L300,222L300,228.5L311.2,217.5Z"" style=""fill:white;fill-rule:nonzero;""/>
    </g>
    <g id=""Table-Add"" serif:id=""Table Add"" transform=""matrix(1.31818,1.62478e-32,1.62478e-32,1.31818,-1509.36,-1811.83)"">
        <g transform=""matrix(1,6.16298e-32,0,1,1146.2,1332.9)"">
            <circle cx=""116.8"" cy=""88.1"" r=""22"" style=""fill:rgb(76,217,100);""/>
        </g>
        <g transform=""matrix(1,6.16298e-32,0,1,1146.2,1332.9)"">
            <path d=""M127.8,87.1L117.8,87.1L117.8,77.1L115.8,77.1L115.8,87.1L105.8,87.1L105.8,89.1L115.8,89.1L115.8,99.1L117.8,99.1L117.8,89.1L127.8,89.1L127.8,87.1Z"" style=""fill:white;fill-rule:nonzero;""/>
        </g>
    </g>
</svg>

";
        }

    }
}