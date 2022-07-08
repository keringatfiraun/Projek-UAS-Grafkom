using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Text;
using LearnOpenTK.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;



namespace pert1
{
    static class Constants
    {
        public const string path = "../../../shader/";
    }
    internal class Window : GameWindow
    {
        private readonly Vector3[] _pointLightPositions =
{
            new Vector3(0.7f, 0.2f, 2.0f),
            new Vector3(2.3f, -3.3f, -4.0f),
            new Vector3(-4.0f, 2.0f, -12.0f),
            new Vector3(0.0f, 0.0f, -3.0f)
        };

        Asset3d[] _object3d = new Asset3d[75];
        Asset3d lightobject = new Asset3d();
        Asset3d[] LightObjects = new Asset3d[4];
        List<Asset3d> _objectObject = new List<Asset3d>();
        List<Asset3d> _objectCharacter = new List<Asset3d>();
        List<Asset3d> _objectJalan = new List<Asset3d>();
        double _time;
        float degr = 0;
        Camera _camera;
        Vector3 _objecPost = new Vector3(0.0f, 0.0f, 0.0f);
        float _rotationSpeed = 1f;
        bool move = true;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            
        }
        public Matrix4 generateArbRotationMatrix(Vector3 axis, Vector3 center, float degree)
        {
            var rads = MathHelper.DegreesToRadians(degree);

            var secretFormula = new float[4, 4] {
                { (float)Math.Cos(rads) + (float)Math.Pow(axis.X, 2) * (1 - (float)Math.Cos(rads)), axis.X* axis.Y * (1 - (float)Math.Cos(rads)) - axis.Z * (float)Math.Sin(rads),    axis.X * axis.Z * (1 - (float)Math.Cos(rads)) + axis.Y * (float)Math.Sin(rads),   0 },
                { axis.Y * axis.X * (1 - (float)Math.Cos(rads)) + axis.Z * (float)Math.Sin(rads),   (float)Math.Cos(rads) + (float)Math.Pow(axis.Y, 2) * (1 - (float)Math.Cos(rads)), axis.Y * axis.Z * (1 - (float)Math.Cos(rads)) - axis.X * (float)Math.Sin(rads),   0 },
                { axis.Z * axis.X * (1 - (float)Math.Cos(rads)) - axis.Y * (float)Math.Sin(rads),   axis.Z * axis.Y * (1 - (float)Math.Cos(rads)) + axis.X * (float)Math.Sin(rads),   (float)Math.Cos(rads) + (float)Math.Pow(axis.Z, 2) * (1 - (float)Math.Cos(rads)), 0 },
                { 0, 0, 0, 1}
            };
            var secretFormulaMatix = new Matrix4
            (
                new Vector4(secretFormula[0, 0], secretFormula[0, 1], secretFormula[0, 2], secretFormula[0, 3]),
                new Vector4(secretFormula[1, 0], secretFormula[1, 1], secretFormula[1, 2], secretFormula[1, 3]),
                new Vector4(secretFormula[2, 0], secretFormula[2, 1], secretFormula[2, 2], secretFormula[2, 3]),
                new Vector4(secretFormula[3, 0], secretFormula[3, 1], secretFormula[3, 2], secretFormula[3, 3])
            );

            return secretFormulaMatix;
        }
        protected override void OnLoad()
        {

            _camera = new Camera(new Vector3(0, 0, 16), Size.X / (float)Size.Y);
            base.OnLoad();
            //ganti background
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            lightobject.createEllipsoid2(0.5f, 1.75f, 1, 0.3f, 0.3f, 0.3f, 72, 24);
            lightobject.load(Constants.path + "shader.vert", Constants.path + "shader.frag", Size.X, Size.Y);

            //character
            //kepala
            _object3d[0] = new Asset3d();
            _object3d[0].createBoxVertices2(0.23f, 0.18f, 0.25f, 0.05f);
            _object3d[0].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectCharacter.Add(_object3d[0]);

            _object3d[1] = new Asset3d();
            _object3d[1].createBoxVertices2(0.28f, 0.18f, 0.25f, 0.05f);
            _object3d[1].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectCharacter.Add(_object3d[1]);

            _object3d[2] = new Asset3d();
            _object3d[2].createBoxVertices2(0.25f, 0.065f, 0.25f, 0.15f);
            _object3d[2].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectCharacter.Add(_object3d[2]);

            //badan
            _object3d[3] = new Asset3d();
            _object3d[3].createBoxVertices2(0.25f, 0.09f, 0.25f, 0.15f);
            _object3d[3].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectCharacter.Add(_object3d[3]);

            _object3d[4] = new Asset3d();
            _object3d[4].createBoxVertices2(0.35f, -0.06f, 0.25f, 0.15f);
            _object3d[4].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectCharacter.Add(_object3d[4]);

            _object3d[5] = new Asset3d();
            _object3d[5].createBoxVertices2(0.4f, -0.06f, 0.25f, 0.10f);
            _object3d[5].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectCharacter.Add(_object3d[5]);

            //mulut
            _object3d[6] = new Asset3d();
            _object3d[6].createBoxVertices2(0.15f, 0.07f, 0.25f, 0.05f);
            _object3d[6].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectCharacter.Add(_object3d[6]);

            _object3d[7] = new Asset3d();
            _object3d[7].createBoxVertices2(0.13f, 0.07f, 0.25f, 0.05f);
            _object3d[7].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectCharacter.Add(_object3d[7]);

            _object3d[8] = new Asset3d();
            _object3d[8].createBoxVertices2(0.15f, 0.02f, 0.25f, 0.05f);
            _object3d[8].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectCharacter.Add(_object3d[8]);

            //sayap kiri
            _object3d[9] = new Asset3d();
            _object3d[9].createBoxVertices2(0.25f, -0.06f, 0.2f, 0.12f);
            _object3d[9].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectCharacter.Add(_object3d[9]);

            //sayap kanan
            _object3d[10] = new Asset3d();
            _object3d[10].createBoxVertices2(0.25f, -0.06f, 0.3f, 0.12f);
            _object3d[10].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectCharacter.Add(_object3d[10]);

            //kaki kiri
            _object3d[11] = new Asset3d();
            _object3d[11].createBoxVertices2(0.32f, -0.16f, 0.285f, 0.04f);
            _object3d[11].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectCharacter.Add(_object3d[11]);

            _object3d[12] = new Asset3d();
            _object3d[12].createBoxVertices2(0.32f, -0.2f, 0.285f, 0.04f);
            _object3d[12].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectCharacter.Add(_object3d[12]);

            _object3d[13] = new Asset3d();
            _object3d[13].createBoxVertices2(0.28f, -0.2f, 0.285f, 0.04f);
            _object3d[13].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectCharacter.Add(_object3d[13]);

            //kaki kanan
            _object3d[14] = new Asset3d();
            _object3d[14].createBoxVertices2(0.32f, -0.16f, 0.215f, 0.04f);
            _object3d[14].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectCharacter.Add(_object3d[14]);

            _object3d[15] = new Asset3d();
            _object3d[15].createBoxVertices2(0.32f, -0.2f, 0.215f, 0.04f);
            _object3d[15].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectCharacter.Add(_object3d[15]);

            _object3d[16] = new Asset3d();
            _object3d[16].createBoxVertices2(0.28f, -0.2f, 0.215f, 0.04f);
            _object3d[16].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectCharacter.Add(_object3d[16]);

            //mata kiri
            _object3d[17] = new Asset3d();
            _object3d[17].createBoxVertices2(0.21f, 0.115f, 0.32f, 0.03f);
            _object3d[17].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectCharacter.Add(_object3d[17]);

            //mata kanan
            _object3d[18] = new Asset3d();
            _object3d[18].createBoxVertices2(0.21f, 0.115f, 0.18f, 0.03f);
            _object3d[18].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectCharacter.Add(_object3d[18]);

            //GEDUNG 1

            _object3d[19] = new Asset3d();
            _object3d[19].createBoxVertices2(1.5f, 0.0f, 0.5f, 0.5f);
            _object3d[19].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectObject.Add(_object3d[19]);
            _object3d[20] = new Asset3d();
            _object3d[20].createBoxVertices2(1.5f, 0.5f, 0.5f, 0.5f);
            _object3d[20].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectObject.Add(_object3d[20]);

            //Jendela
            _object3d[23] = new Asset3d();
            _object3d[23].createBoxVertices2(1.285f, 0.6f, -0.615f, 0.115f);
            _object3d[23].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectObject.Add(_object3d[23]);
            _object3d[24] = new Asset3d();
            _object3d[24].createBoxVertices2(1.285f, 0.6f, -0.4f, 0.115f);
            _object3d[24].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectObject.Add(_object3d[24]);

            _object3d[25] = new Asset3d();
            _object3d[25].createBoxVertices2(1.285f, 0.45f, -0.615f, 0.115f);
            _object3d[25].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectObject.Add(_object3d[25]);
            _object3d[26] = new Asset3d();
            _object3d[26].createBoxVertices2(1.285f, 0.45f, -0.4f, 0.115f);
            _object3d[26].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectObject.Add(_object3d[26]);

            _object3d[27] = new Asset3d();
            _object3d[27].createBoxVertices2(1.285f, 0.3f, -0.615f, 0.115f);
            _object3d[27].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectObject.Add(_object3d[27]);
            _object3d[28] = new Asset3d();
            _object3d[28].createBoxVertices2(1.285f, 0.3f, -0.4f, 0.115f);
            _object3d[28].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectObject.Add(_object3d[28]);

            _object3d[29] = new Asset3d();
            _object3d[29].createBoxVertices2(1.285f, 0.15f, -0.615f, 0.115f);
            _object3d[29].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectObject.Add(_object3d[29]);
            _object3d[30] = new Asset3d();
            _object3d[30].createBoxVertices2(1.285f, 0.15f, -0.4f, 0.115f);
            _object3d[30].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectObject.Add(_object3d[30]);

            //Pintu
            _object3d[31] = new Asset3d();
            _object3d[31].createBoxVertices2(1.285f, -0.075f, -0.5f, 0.11f);
            _object3d[31].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectObject.Add(_object3d[31]);
            _object3d[32] = new Asset3d();
            _object3d[32].createBoxVertices2(1.285f, -0.185f, -0.5f, 0.11f);
            _object3d[32].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectObject.Add(_object3d[32]);

            //GEDUNG 2
            _object3d[21] = new Asset3d();
            _object3d[21].createBoxVertices2(1.5f, 0.0f, -0.5f, 0.5f);
            _object3d[21].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectObject.Add(_object3d[21]);
            _object3d[22] = new Asset3d();
            _object3d[22].createBoxVertices2(1.5f, 0.5f, -0.5f, 0.5f);
            _object3d[22].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectObject.Add(_object3d[22]);

            _object3d[33] = new Asset3d();
            _object3d[33].createBoxVertices2(1.285f, 0.6f, 0.615f, 0.115f);
            _object3d[33].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectObject.Add(_object3d[33]);
            _object3d[34] = new Asset3d();
            _object3d[34].createBoxVertices2(1.285f, 0.6f, 0.4f, 0.115f);
            _object3d[34].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectObject.Add(_object3d[34]);

            _object3d[35] = new Asset3d();
            _object3d[35].createBoxVertices2(1.285f, 0.45f, 0.615f, 0.115f);
            _object3d[35].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectObject.Add(_object3d[35]);
            _object3d[36] = new Asset3d();
            _object3d[36].createBoxVertices2(1.285f, 0.45f, 0.4f, 0.115f);
            _object3d[36].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectObject.Add(_object3d[36]);

            _object3d[37] = new Asset3d();
            _object3d[37].createBoxVertices2(1.285f, 0.3f, 0.615f, 0.115f);
            _object3d[37].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectObject.Add(_object3d[37]);
            _object3d[38] = new Asset3d();
            _object3d[38].createBoxVertices2(1.285f, 0.3f, 0.4f, 0.115f);
            _object3d[38].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectObject.Add(_object3d[38]);

            _object3d[39] = new Asset3d();
            _object3d[39].createBoxVertices2(1.285f, 0.15f, 0.615f, 0.115f);
            _object3d[39].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectObject.Add(_object3d[39]);
            _object3d[40] = new Asset3d();
            _object3d[40].createBoxVertices2(1.285f, 0.15f, 0.4f, 0.115f);
            _object3d[40].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectObject.Add(_object3d[40]);

            _object3d[41] = new Asset3d();
            _object3d[41].createBoxVertices2(1.285f, -0.075f, 0.5f, 0.115f);
            _object3d[41].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectObject.Add(_object3d[41]);
            _object3d[42] = new Asset3d();
            _object3d[42].createBoxVertices2(1.285f, -0.185f, 0.5f, 0.115f);
            _object3d[42].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectObject.Add(_object3d[42]);

            //Jalan
            _object3d[43] = new Asset3d();
            _object3d[43].createBoxVertices3(1.25f, -0.25f, 0, -2f, 0.1f, -3f);
            _object3d[43].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectJalan.Add(_object3d[43]);

            _object3d[44] = new Asset3d();
            _object3d[44].createBoxVertices3(-0.45f, -0.25f, 0, -1.4f, 0.1f, -3f);
            _object3d[44].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectJalan.Add(_object3d[44]);

            _object3d[62] = new Asset3d();
            _object3d[62].createBoxVertices3(-2f, -0.25f, 0, -1.7f, 0.1f, -3f);
            _object3d[62].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectJalan.Add(_object3d[62]);

            //garis putih
            _object3d[51] = new Asset3d();
            _object3d[51].createBoxVertices3(0.15f, -0.245f, 0f, -0.05f, 0.1f, -3f);
            _object3d[51].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectJalan.Add(_object3d[51]);

            _object3d[52] = new Asset3d();
            _object3d[52].createBoxVertices3(-1.05f, -0.245f, 0f, -0.05f, 0.1f, -3f);
            _object3d[52].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectJalan.Add(_object3d[52]);

            _object3d[53] = new Asset3d();
            _object3d[53].createBoxVertices3(-0.375f, -0.245f, 1.25f, -0.05f, 0.1f, -0.3f);
            _object3d[53].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectJalan.Add(_object3d[53]);

            _object3d[54] = new Asset3d();
            _object3d[54].createBoxVertices3(-0.375f, -0.245f, 0.5f, -0.05f, 0.1f, -0.3f);
            _object3d[54].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectJalan.Add(_object3d[54]);

            _object3d[55] = new Asset3d();
            _object3d[55].createBoxVertices3(-0.375f, -0.245f, -0.25f, -0.05f, 0.1f, -0.3f);
            _object3d[55].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectJalan.Add(_object3d[55]);

            _object3d[56] = new Asset3d();
            _object3d[56].createBoxVertices3(-0.375f, -0.245f, -1f, -0.05f, 0.1f, -0.3f);
            _object3d[56].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectJalan.Add(_object3d[56]);

            //Pohon//x -> z
            _object3d[45] = new Asset3d();
            _object3d[45].createBoxVertices3(1.25f, 0f, 1.3f, -0.075f, 0.4f, -0.075f);
            _object3d[45].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectJalan.Add(_object3d[45]);

            //p -> lebar?
            //l -> tinggi
            //t -> panjang?
            _object3d[46] = new Asset3d();
            _object3d[46].createBoxVertices3(1.235f, 0.075f, 1.215f, -0.035f, 0.04f, -0.1f);
            _object3d[46].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectJalan.Add(_object3d[46]);

            _object3d[47] = new Asset3d();
            _object3d[47].createBoxVertices3(1.235f, 0.1055f, 1.15f, -0.035f, 0.1f, -0.07f);
            _object3d[47].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectJalan.Add(_object3d[47]);

            //Daun
            _object3d[48] = new Asset3d();
            _object3d[48].createBoxVertices2(1.285f, 0.275f, 1.3f, 0.3f);
            _object3d[48].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectObject.Add(_object3d[48]);

            _object3d[49] = new Asset3d();
            _object3d[49].createBoxVertices2(1.25f, 0.23f, 1.15f, 0.15f);
            _object3d[49].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectObject.Add(_object3d[49]);

            //Pohon 2
            _object3d[57] = new Asset3d();
            _object3d[57].createBoxVertices3(1.25f, 0f, -1.25f, -0.075f, 0.4f, -0.075f);
            _object3d[57].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectObject.Add(_object3d[57]);

            //p -> lebar?
            //l -> tinggi
            //t -> panjang?
            _object3d[58] = new Asset3d();
            _object3d[58].createBoxVertices3(1.235f, 0.075f, -1.335f, -0.035f, 0.04f, -0.1f);
            _object3d[58].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectObject.Add(_object3d[58]);

            _object3d[59] = new Asset3d();
            _object3d[59].createBoxVertices3(1.235f, 0.1055f, -1.4f, -0.035f, 0.1f, -0.07f);
            _object3d[59].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectObject.Add(_object3d[59]);

            //Daun
            _object3d[60] = new Asset3d();
            _object3d[60].createBoxVertices2(1.285f, 0.275f, -1.25f, 0.3f);
            _object3d[60].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectObject.Add(_object3d[60]);

            _object3d[61] = new Asset3d();
            _object3d[61].createBoxVertices2(1.25f, 0.23f, -1.4f, 0.15f);
            _object3d[61].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectObject.Add(_object3d[61]);

            //LAMPU
            _object3d[50] = new Asset3d();
            _object3d[50].createBoxVertices3(1.2f, -0.1f, 0f, -0.05f, 0.4f, -0.05f);
            _object3d[50].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectObject.Add(_object3d[50]);

            _object3d[69] = new Asset3d();
            _object3d[69].createBoxVertices3(-1.5f, -0.1f, 0f, -0.05f, 0.4f, -0.05f);
            _object3d[69].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectObject.Add(_object3d[69]);

            //gedung 3
            _object3d[63] = new Asset3d();
            _object3d[63].createBoxVertices2(-2f, 0.0f, 0.6f, 0.5f);
            _object3d[63].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectObject.Add(_object3d[63]);
            _object3d[64] = new Asset3d();
            _object3d[64].createBoxVertices2(-2f, 0.5f, 0.6f, 0.5f);
            _object3d[64].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectObject.Add(_object3d[64]);
            _object3d[65] = new Asset3d();
            _object3d[65].createBoxVertices2(-2f, 1f, 0.6f, 0.5f);
            _object3d[65].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectObject.Add(_object3d[65]);

            _object3d[66] = new Asset3d();
            _object3d[66].createBoxVertices3(-1.75f, 0.615f, 0.415f, -0.05f, 0.85f, -0.065f);
            _object3d[66].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectObject.Add(_object3d[66]);

            _object3d[67] = new Asset3d();
            _object3d[67].createBoxVertices3(-1.75f, 0.615f, 0.58f, -0.05f, 0.85f, -0.065f);
            _object3d[67].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectObject.Add(_object3d[67]);

            _object3d[68] = new Asset3d();
            _object3d[68].createBoxVertices3(-1.75f, 0.615f, 0.76f, -0.05f, 0.85f, -0.065f);
            _object3d[68].load_withnormal(Constants.path + "objectcolor.vert", Constants.path + "objectcolor.frag", Size.X, Size.Y);
            _objectObject.Add(_object3d[68]);


            _camera = new Camera(new Vector3(0, 0, 1), Size.X / Size.Y);

            LightObjects[0] = new Asset3d();
            LightObjects[0].createEllipsoid2(1.2f, 0.2f, 0f, 0.1f, 0.1f, 0.1f, 72, 24);
            LightObjects[0].load(Constants.path + "shader.vert", Constants.path + "shader.frag", Size.X, Size.Y);

            LightObjects[1] = new Asset3d();
            LightObjects[1].createEllipsoid2(-1.5f, 0.2f, 0f, 0.1f, 0.1f, 0.1f, 72, 24);
            LightObjects[1].load(Constants.path + "shader.vert", Constants.path + "shader.frag", Size.X, Size.Y);

            //CursorGrabbed = true;
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit| ClearBufferMask.DepthBufferBit);
            Matrix4 temp = Matrix4.Identity;
            _time += 9.0 * args.Time;
            degr += MathHelper.DegreesToRadians(0.5f);

            lightobject.render(0, _time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());

            _objectCharacter[0].setFragVariabel2(new Vector3(1.0f, 0.2f, 0.5f), _camera.Position);
            _objectCharacter[1].setFragVariabel2(new Vector3(1.0f, 0.2f, 0.5f), _camera.Position);
            _objectCharacter[2].setFragVariabel2(new Vector3(1.0f, 0.8f, 0.4f), _camera.Position);
            _objectCharacter[3].setFragVariabel2(new Vector3(1.0f, 0.8f, 0.4f), _camera.Position);
            _objectCharacter[4].setFragVariabel2(new Vector3(1.0f, 0.8f, 0.4f), _camera.Position);
            _objectCharacter[5].setFragVariabel2(new Vector3(1.0f, 0.8f, 0.4f), _camera.Position);
            _objectCharacter[6].setFragVariabel2(new Vector3(1.0f, 0.6f, 0f), _camera.Position);
            _objectCharacter[7].setFragVariabel2(new Vector3(1.0f, 0.6f, 0f), _camera.Position);
            _objectCharacter[8].setFragVariabel2(new Vector3(1.0f, 0.2f, 0.5f), _camera.Position);
            _objectCharacter[9].setFragVariabel2(new Vector3(1.0f, 0.8f, 0.4f), _camera.Position);
            _objectCharacter[10].setFragVariabel2(new Vector3(1.0f, 0.8f, 0.4f), _camera.Position);
            _objectCharacter[11].setFragVariabel2(new Vector3(1.0f, 0.6f, 0f), _camera.Position);
            _objectCharacter[12].setFragVariabel2(new Vector3(1.0f, 0.6f, 0f), _camera.Position);
            _objectCharacter[13].setFragVariabel2(new Vector3(1.0f, 0.6f, 0f), _camera.Position);
            _objectCharacter[14].setFragVariabel2(new Vector3(1.0f, 0.6f, 0f), _camera.Position);
            _objectCharacter[15].setFragVariabel2(new Vector3(1.0f, 0.6f, 0f), _camera.Position);
            _objectCharacter[16].setFragVariabel2(new Vector3(1.0f, 0.6f, 0f), _camera.Position);
            _objectCharacter[17].setFragVariabel2(new Vector3(0f, 0f, 0f), _camera.Position);
            _objectCharacter[18].setFragVariabel2(new Vector3(0f, 0f, 0f), _camera.Position);

            _object3d[19].setFragVariabel2(new Vector3(0.6f, 0.6f, 0.9f), _camera.Position);
            _object3d[20].setFragVariabel2(new Vector3(0.6f, 0.6f, 0.9f), _camera.Position);
            _object3d[21].setFragVariabel2(new Vector3(0.7f, 0.1f, 0.1f), _camera.Position);
            _object3d[22].setFragVariabel2(new Vector3(0.7f, 0.1f, 0.1f), _camera.Position);
            _object3d[23].setFragVariabel2(new Vector3(0.6f, 1f, 1f), _camera.Position);
            _object3d[24].setFragVariabel2(new Vector3(0.6f, 1f, 1f), _camera.Position);
            _object3d[25].setFragVariabel2(new Vector3(0.6f, 1f, 1f), _camera.Position);
            _object3d[26].setFragVariabel2(new Vector3(0.6f, 1f, 1f), _camera.Position);
            _object3d[27].setFragVariabel2(new Vector3(0.6f, 1f, 1f), _camera.Position);
            _object3d[28].setFragVariabel2(new Vector3(0.6f, 1f, 1f), _camera.Position);
            _object3d[29].setFragVariabel2(new Vector3(0.6f, 1f, 1f), _camera.Position);
            _object3d[30].setFragVariabel2(new Vector3(0.6f, 1f, 1f), _camera.Position);
            _object3d[31].setFragVariabel2(new Vector3(0.6f, 1f, 1f), _camera.Position);
            _object3d[32].setFragVariabel2(new Vector3(0.6f, 1f, 1f), _camera.Position);
            _object3d[33].setFragVariabel2(new Vector3(0.9f, 1f, 0.8f), _camera.Position);
            _object3d[34].setFragVariabel2(new Vector3(0.9f, 1f, 0.8f), _camera.Position);
            _object3d[35].setFragVariabel2(new Vector3(0.9f, 1f, 0.8f), _camera.Position);
            _object3d[36].setFragVariabel2(new Vector3(0.9f, 1f, 0.8f), _camera.Position);
            _object3d[37].setFragVariabel2(new Vector3(0.9f, 1f, 0.8f), _camera.Position);
            _object3d[38].setFragVariabel2(new Vector3(0.9f, 1f, 0.8f), _camera.Position);
            _object3d[39].setFragVariabel2(new Vector3(0.9f, 1f, 0.8f), _camera.Position);
            _object3d[40].setFragVariabel2(new Vector3(0.9f, 1f, 0.8f), _camera.Position);
            _object3d[41].setFragVariabel2(new Vector3(0.9f, 1f, 0.8f), _camera.Position);
            _object3d[42].setFragVariabel2(new Vector3(0.9f, 1f, 0.8f), _camera.Position);
            //jalan
            _object3d[43].setFragVariabel2(new Vector3(0.6f, 0.7f, 0.8f), _camera.Position);
            _object3d[44].setFragVariabel2(new Vector3(0.4f, 0.5f, 0.6f), _camera.Position);
            //batang
            _object3d[45].setFragVariabel2(new Vector3(1f, 0.2f, 0f), _camera.Position);
            _object3d[46].setFragVariabel2(new Vector3(1f, 0.2f, 0f), _camera.Position);
            _object3d[47].setFragVariabel2(new Vector3(1f, 0.2f, 0f), _camera.Position);
            _object3d[48].setFragVariabel2(new Vector3(0.1f, 0.5f, 0.4f), _camera.Position);
            _object3d[49].setFragVariabel2(new Vector3(0.1f, 0.5f, 0.4f), _camera.Position);
            //Lampu
            _object3d[50].setFragVariabel2(new Vector3(0.3f, 0.2f, 0.1f), _camera.Position);
            //jalan
            _object3d[51].setFragVariabel2(new Vector3(1f, 1f, 1f), _camera.Position);
            _object3d[52].setFragVariabel2(new Vector3(1f, 1f, 1f), _camera.Position);
            _object3d[53].setFragVariabel2(new Vector3(1f, 1f, 1f), _camera.Position);
            _object3d[54].setFragVariabel2(new Vector3(1f, 1f, 1f), _camera.Position);
            _object3d[55].setFragVariabel2(new Vector3(1f, 1f, 1f), _camera.Position);
            _object3d[56].setFragVariabel2(new Vector3(1f, 1f, 1f), _camera.Position);

            //Pohon 2
            _object3d[57].setFragVariabel2(new Vector3(1f, 0.2f, 0f), _camera.Position);
            _object3d[58].setFragVariabel2(new Vector3(1f, 0.2f, 0f), _camera.Position);
            _object3d[59].setFragVariabel2(new Vector3(1f, 0.2f, 0f), _camera.Position);
            _object3d[60].setFragVariabel2(new Vector3(0.1f, 0.5f, 0.4f), _camera.Position);
            _object3d[61].setFragVariabel2(new Vector3(0.1f, 0.5f, 0.4f), _camera.Position);
            //
            _object3d[62].setFragVariabel2(new Vector3(0.6f, 0.7f, 0.8f), _camera.Position);
            //
            _object3d[63].setFragVariabel2(new Vector3(0f, 0f, 1f), _camera.Position);
            _object3d[64].setFragVariabel2(new Vector3(0f, 0f, 1f), _camera.Position);
            _object3d[65].setFragVariabel2(new Vector3(0f, 0f, 1f), _camera.Position);
            _object3d[66].setFragVariabel2(new Vector3(0.7f, 1f, 1f), _camera.Position);
            _object3d[67].setFragVariabel2(new Vector3(0.7f, 1f, 1f), _camera.Position);
            _object3d[68].setFragVariabel2(new Vector3(0.7f, 1f, 1f), _camera.Position);
            //Lampu2
            _object3d[69].setFragVariabel2(new Vector3(0.3f, 0.2f, 0.1f), _camera.Position);


            foreach (Asset3d i in _objectCharacter)
            {
                i.render(0, _time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
                i.setDirectionalLight(new Vector3(-0.2f, -1.0f, -0.3f), new Vector3(0.05f, 0.05f, 0.05f), new Vector3(0.4f, 0.4f, 0.4f), 
                    new Vector3(0.5f, 0.5f, 0.5f));
                //mengikuti camera
                i.setSpotLight(_objectCharacter[0]._centerPosition, _camera.Front, new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f), 
                    new Vector3(1.0f, 1.0f, 1.0f),
                    1.0f, 0.09f, 0.032f, MathF.Cos(MathHelper.DegreesToRadians(12.5f)), MathF.Cos(MathHelper.DegreesToRadians(12.5f)));
                // multi point light
                i.setPointLights(_pointLightPositions, new Vector3(0.05f, 0.05f, 0.05f), new Vector3(0.8f, 0.8f, 0.8f), 
                    new Vector3(1.0f, 1.0f, 1.0f), 1.0f, 0.09f, 0.032f);
            }

            foreach (Asset3d i in _objectObject)
            {
                i.render(0, _time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
                i.setDirectionalLight(new Vector3(-0.2f, -1.0f, -0.3f), new Vector3(0.05f, 0.05f, 0.05f), new Vector3(0.4f, 0.4f, 0.4f), new Vector3(0.5f, 0.5f, 0.5f));
                //mengikuti camera
                i.setSpotLight(_objectCharacter[0]._centerPosition, _camera.Front, new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f),
                    1.0f, 0.09f, 0.032f, MathF.Cos(MathHelper.DegreesToRadians(12.5f)), MathF.Cos(MathHelper.DegreesToRadians(12.5f)));
                // multi point light
                i.setPointLights(_pointLightPositions, new Vector3(0.05f, 0.05f, 0.05f), new Vector3(0.8f, 0.8f, 0.8f), new Vector3(1.0f, 1.0f, 1.0f), 1.0f, 0.09f, 0.032f);
            }

            foreach (Asset3d i in _objectJalan)
            {

                i.render(0, _time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
                i.setDirectionalLight(new Vector3(-0.2f, -1.0f, -0.3f), new Vector3(0.05f, 0.05f, 0.05f), new Vector3(0.4f, 0.4f, 0.4f), new Vector3(0.5f, 0.5f, 0.5f));
                //mengikuti camera
                i.setSpotLight(_objectCharacter[0]._centerPosition, _camera.Front, new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f),
                    1.0f, 0.09f, 0.032f, MathF.Cos(MathHelper.DegreesToRadians(12.5f)), MathF.Cos(MathHelper.DegreesToRadians(12.5f)));
                // multi point light
                i.setPointLights(_pointLightPositions, new Vector3(0.05f, 0.05f, 0.05f), new Vector3(0.8f, 0.8f, 0.8f), new Vector3(1.0f, 1.0f, 1.0f), 1.0f, 0.09f, 0.032f);
            }

            LightObjects[0].render(0, _time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
            LightObjects[1].render(0, _time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
            SwapBuffers();
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            _camera.Fov = _camera.Fov - e.OffsetY;
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
            _camera.AspectRatio = Size.X / (float)Size.Y;
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            var input = KeyboardState;
            float cameraSpeed = 3f;
            
            //character.minMax();
            Vector3 _objectPos = new Vector3(_objectCharacter[3]._centerPosition.X, _objectCharacter[3]._centerPosition.Y, _objectCharacter[2]._centerPosition.Z);

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            if (KeyboardState.IsKeyDown(Keys.W))
            {
                if (move == true)
                {
                    _camera.Position += _camera.Front * cameraSpeed * (float)args.Time;
                    foreach (Asset3d i in _objectCharacter)
                    {
                        i.translate2(_camera.Front * cameraSpeed * (float)args.Time);
                    }
                }
                else
                {
                    _camera.Position += _camera.Front * cameraSpeed * (float)args.Time;
                }
                _camera.Pitch = 0;
            }
            if (KeyboardState.IsKeyDown(Keys.S))
            {
                if (move == true)
                {
                    _camera.Position -= _camera.Front * cameraSpeed * (float)args.Time;
                    foreach (Asset3d i in _objectCharacter)
                    {
                        i.translate2(-_camera.Front * cameraSpeed * (float)args.Time);
                    }
                }
                else
                {
                    _camera.Position -= _camera.Front * cameraSpeed * (float)args.Time;
                }
                _camera.Pitch = 0;
            }
            if (KeyboardState.IsKeyDown(Keys.A))
            {

                if (move == true)
                {
                    _camera.Position -= _camera.Right * cameraSpeed * (float)args.Time;
                    foreach (Asset3d i in _objectCharacter)
                    {

                        i.translate2(-_camera.Right * cameraSpeed * (float)args.Time);
                    }
                }
                else
                {
                    _camera.Position -= _camera.Right * cameraSpeed * (float)args.Time;
                }
            }
            if (KeyboardState.IsKeyDown(Keys.D))
            {
                if (move == true)
                {
                    _camera.Position += _camera.Right * cameraSpeed * (float)args.Time;
                    foreach (Asset3d i in _objectCharacter)
                    {

                        i.translate2(_camera.Right * cameraSpeed * (float)args.Time);
                    }
                }
                else
                {
                    _camera.Position += _camera.Right * cameraSpeed * (float)args.Time;
                }

            }
            if (KeyboardState.IsKeyDown(Keys.Up))
            {
                _camera.Position += _camera.Up * cameraSpeed * (float)args.Time;
                foreach (Asset3d i in _objectCharacter)
                {
                    i.translate2(_camera.Up * cameraSpeed * (float)args.Time);
                }
            }
            if (KeyboardState.IsKeyDown(Keys.Down))
            {
                _camera.Position -= _camera.Up * cameraSpeed * (float)args.Time;
                foreach (Asset3d i in _objectCharacter)
                {
                    i.translate2(-_camera.Up * cameraSpeed * (float)args.Time);
                }
            }

            if (KeyboardState.IsKeyPressed(Keys.G))
            {
                move = !move;
            }
            var mouse = MouseState;
            var sensitivity = 0.2f;

            //if (_firstMove)
            //{
            //    _lastPos = new Vector2(mouse.X, mouse.Y);
            //    _firstMove = false;
            //}
            //else
            //{
            //    var deltaX = mouse.X - _lastPos.X;
            //    var deltaY = mouse.Y - _lastPos.Y;
            //    _lastPos = new Vector2(mouse.X, mouse.Y);
            //    _camera.Yaw += deltaX * sensitivity;
            //    _camera.Pitch -= deltaY * sensitivity;
            //}

            if (KeyboardState.IsKeyDown(Keys.N))
            {
                var axis = new Vector3(0, 1.5f, 0);
                _camera.Position -= _objectPos;
                _camera.Yaw -= _rotationSpeed;
                _camera.Position = Vector3.Transform(_camera.Position, generateArbRotationMatrix(axis, _objectPos, -_rotationSpeed).ExtractRotation());
                _camera.Position += _objectPos;
                _camera._front = -Vector3.Normalize(_camera.Position - _objectPos);
                foreach (Asset3d i in _objectCharacter)
                {
                    i.rotate(_objectPos, Vector3.UnitY, _rotationSpeed);
                }
            }
            if (KeyboardState.IsKeyDown(Keys.Comma))
            {
                var axis = new Vector3(0, 1, 0);
                _camera.Position -= _objectPos;
                _camera.Yaw += _rotationSpeed;
                _camera.Position = Vector3.Transform(_camera.Position, generateArbRotationMatrix(axis, _objectPos, _rotationSpeed).ExtractRotation());
                _camera.Position += _objectPos;
                _camera._front = -Vector3.Normalize(_camera.Position - _objectPos);
                foreach (Asset3d i in _objectCharacter)
                {
                    i.rotate(_objectPos, Vector3.UnitY, -_rotationSpeed);
                }
            }
            /*if (KeyboardState.IsKeyDown(Keys.K))
            {
                var axis = new Vector3(1, 0, 0);
                _camera.Position -= _objectPos;
                _camera.Pitch -= _rotationSpeed;
                _camera.Position = Vector3.Transform(_camera.Position,generateArbRotationMatrix(axis, _objectPos, _rotationSpeed).ExtractRotation());
                _camera.Position += _objectPos;
                _camera._front = -Vector3.Normalize(_camera.Position - _objectPos);
               //character.rotate(character._centerPosition, Vector3.UnitX, -0.05f);
            }
            if (KeyboardState.IsKeyDown(Keys.M))
            {
                var axis = new Vector3(1, 0, 0);
                _camera.Position -= _objectPos;
                _camera.Pitch += _rotationSpeed;
                _camera.Position = Vector3.Transform(_camera.Position,
                    generateArbRotationMatrix(axis, _objectPos, -_rotationSpeed).ExtractRotation());
                _camera.Position += _objectPos;
                _camera._front = -Vector3.Normalize(_camera.Position - _objectPos);
                //character.rotate(character._centerPosition, Vector3.UnitX, 0.05f);
            }*/
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButton.Left)
            {
                float _x = (MousePosition.X - Size.X / 2) / (Size.X / 2);
                float _y = -(MousePosition.Y - Size.Y / 2) / (Size.Y / 2);

                Console.WriteLine("x = " + _x + "y = " + _y);
                //_object[1].updateMousePotition(_x, _y);
            }
        }

        public bool isOverlaps()
        {
            foreach (Asset3d i in _object3d)
            {
                var cubeB_max = i.posMax;
                var cubeB_min = i.posMin;

                if (_camera.Position.X >= cubeB_min.X - (cubeB_min.X / .5f) && _camera.Position.X <= cubeB_max.X + (cubeB_min.X / 7.5f))
                {
                    if (_camera.Position.Y >= cubeB_min.Y - (cubeB_min.Y / .5f) && _camera.Position.Y <= cubeB_max.Y + (cubeB_min.Y / .75f))
                    {
                        if (_camera.Position.Z <= cubeB_min.Z + 11.75f && _camera.Position.Z >= cubeB_max.Z - 11.75f)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

    }
}