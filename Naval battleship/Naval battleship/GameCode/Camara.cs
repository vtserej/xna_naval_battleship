using System;
using System.Collections.Generic;
using System.Text;
using NavalBattleship.Controls;
using Xengine;
using NavalBattleship.Models;
using Microsoft.Xna.Framework;
using Xengine.WindowsControls;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;   

namespace NavalBattleship
{
    public enum CameraMove
    { Left, Right, Up, Down, ZoomIn, ZoomOut, None };

    public enum CameraMatrixSelected
    {World, View, Projection };

    public enum CameraType
    { Free, Top, Misile};

    public class Camara
    {       
        #region Private atributes

        #region variables de explosion

        static float shakeX, shakeY, shakeZ;
        static bool shake;
        static int shakeCount;
        static float shakeIntensity = 0.15f;
        static int shakeDirection = 1;
        static Random randomizer = new Random();

        #endregion

        static CameraMatrixSelected selectedMatrix = CameraMatrixSelected.View;
        static CameraType cameraType;

        static Matrix view;    
        static Matrix projection;
        static Matrix world;

        static Stack<Matrix> viewSave = new Stack<Matrix>();
        static Stack<Matrix> projectionSave = new Stack<Matrix>();
        static Stack<Matrix> worldSave = new Stack<Matrix>();     

        static float ratio;
        static float nearClip = 0.05f;
        static float farClip = 1000.0f;
        static float fov = MathHelper.PiOver4;

        static Vector3 position;
        static Vector3 center;      
        static Vector3 movement;
        static Vector3 forwardVector;

        static CameraMove move = CameraMove.None;

        static float[] parameters1 = new float[6];
        static float[] parameters2 = new float[6];
        static float forwardSpeed = 0.15f;
        static float scrollSpeed = 1.7f;
        static float yaw, pitch;
        static float rotationSpeed = 0.8f;
        static double i, j, k;
        static double camaraRadius;
        static bool camaraRadiusChanged = true;
        static bool lockedTop; // para vista superior
        static bool locked; // para explosion 
        static bool lockedMisile; // para cuando misil este en el aire
        static bool lockedMisileCamera; // para cuando la camara siga al misil
        static int camaraDirection = -2;
        static bool rotating;
        static bool scrolling;

        #region variables de posicion global

        //WorldPos
        static int[] viewport = new int[4];

        #endregion

        #endregion

        #region Properties

        public static CameraType CameraType
        {
            get { return Camara.cameraType; }
            set { Camara.cameraType = value; }
        }

        public static Matrix View
        {
            get { return Camara.view; }
            set { Camara.view = value; }
        }

        public static Matrix Projection
        {
            get { return Camara.projection; }
            set { Camara.projection = value; }
        }

        public static Matrix World
        {
            get { return Camara.world; }
            set { Camara.world = value; }
        }

        public static double K
        {
            get { return Camara.k; }
            set { Camara.k = value; }
        }

        public static double J
        {
            get { return Camara.j; }
            set { Camara.j = value; }
        }

        public static double I
        {
            get { return Camara.i; }
            set { Camara.i = value; }
        }

        public static float Yaw
        {
            get 
            {
                return yaw;  
            }
            set
            {
                yaw = value; 
            }
        }

        public static float Pitch
        {
            get { return Camara.pitch; }
            set { Camara.pitch = value; }
        }

        public static bool Locked
        {
            get { return Camara.locked; }
            set { Camara.locked = value; }
        }

        public static bool MisileLocked
        {
            get { return Camara.lockedMisile; }
            set { Camara.lockedMisile = value; }
        }

        public static bool RoofLocked
        {
            get { return Camara.lockedTop; }
            set { Camara.lockedTop = value; }
        }

        public static CameraMove Move
        {
            get { return Camara.move; }
            set { Camara.move = value; }
        }

        public static bool Scrolling
        {
            get { return Camara.scrolling; }
            set { Camara.scrolling = value; }
        }

        public static CameraMatrixSelected SelectedMatrix
        {
            get { return Camara.selectedMatrix; }
            set { Camara.selectedMatrix = value; }
        }

        public static Vector3 Position
        {
            get { return Camara.position; }
            set { Camara.position = value; }
        }

        public static Vector3 ForwardVector
        {
            get { return forwardVector; }
        }

        public static bool LockedMisileCamera
        {
            get { return Camara.lockedMisileCamera; }
            set { Camara.lockedMisileCamera = value; }
        }

        public static bool IsShaking
        {
            get { return Camara.shake; }
        }

        #endregion

        public static void PushMatrix()
        {
            switch (selectedMatrix)
            {
                case CameraMatrixSelected.World:
                    worldSave.Push(world);  
                    break;
                case CameraMatrixSelected.View:
                    viewSave.Push(view); 
                    break;
                case CameraMatrixSelected.Projection:
                    projectionSave.Push(projection);   
                    break;
                default:
                    break;
            } 
        }

        public static void PopMatrix()
        {
            switch (selectedMatrix)
            {
                case CameraMatrixSelected.World:
                    world  = worldSave.Pop();
                    break;
                case CameraMatrixSelected.View:
                    view = viewSave.Pop();
                    break;
                case CameraMatrixSelected.Projection:
                    projection = projectionSave.Pop();
                    break;
                default:
                    break;
            } 
        }

        public void InitCamara()
        {
            ratio = Sprite.Graphics.GraphicsDevice.Viewport.AspectRatio;
            world = Matrix.Identity;
            projection = Matrix.CreatePerspectiveFieldOfView(fov, ratio, nearClip, farClip);

            center = new Vector3(10.5f, 0, 7.5f);
            position = new Vector3(0, 2, 15);
            forwardVector = new Vector3();
            cameraType = CameraType.Free;  
            
            CalcPitch(); 
            Look();
        }

        static void Look()
        {
            view = Matrix.CreateLookAt(position, center, Vector3.Up);
            Sprite.BasicEffect.View = view;
            Sprite.BasicEffect.World = world;
            Sprite.BasicEffect.Projection = projection;  
        }

        public void SaveCamaraPos(int select)
        {
            if (select == 0)
            {
                parameters1[0] = position.X;
                parameters1[1] = position.Y;
                parameters1[2] = position.Z;
                parameters1[3] = center.X;
                parameters1[4] = center.Y;
                parameters1[5] = center.Z;
            }
            else
            {
                parameters2[0] = position.X;
                parameters2[1] = position.Y;
                parameters2[2] = position.Z;
                parameters2[3] = center.X;
                parameters2[4] = center.Y;
                parameters2[5] = center.Z;
            }  
        }

        public void RestoreCamaraPos(int select)
        {
            if (select == 0)
            {
                LocateCamara(parameters1);
            }
            else
            {
                LocateCamara(parameters2);  
            }
        }

        static public void Explosion()
        {
            shakeCount = 0;
            shake = true;
            locked = true; 
        }

        static public void ShakeCamera()
        {
            if (shake == true && !Misile.Locked)
            {
                if (shakeCount != 0)
                {
                    view *= Matrix.CreateTranslation(-shakeX, -shakeY, -shakeZ);
                }
                //--para randomizar la direccion 1 o -1, 1 a la n
                int pos = (int)Math.Pow((-1), randomizer.Next(8));

                shakeX = (float)randomizer.NextDouble() * pos * shakeIntensity;
                shakeY = (float)randomizer.NextDouble() * pos * shakeIntensity;
                shakeZ = (float)randomizer.NextDouble() * pos * shakeIntensity;
                view *= Matrix.CreateTranslation(shakeX, shakeY, shakeZ);

                shakeCount++;
                if (shakeIntensity > 0.45f)
                {
                    shakeDirection = -1;
                }
                shakeIntensity += 0.025f * shakeDirection;
                if (shakeCount == 40)
                {
                    view *= Matrix.CreateTranslation(-shakeX, -shakeY, -shakeZ);
                    shake = false;
                    locked = false; 
                    shakeIntensity = 0.15f;
                    shakeDirection = 1;
                }
            }
        }

        static public void RoofCamera()
        {
            if (!lockedMisileCamera)
            {
                if (cameraType == CameraType.Free)
                {
                    //camara de arriba
                    LocateCamara(new Vector3(12.15f, 25, 7.6f), new Vector3(12.15f, 0, 7.5f));
                    cameraType = CameraType.Top;   
                    lockedTop = true;
                }
                else
                {
                    //camara libre
                    LocateCamara(new Vector3(10.5f, 7, 20), new Vector3(10.5f, 0, 7.5f));
                    cameraType = CameraType.Free; 
                    yaw = 0; 
                    lockedTop = false;
                }
            } 
        }

        static public void LocateCamara(Vector3 position, Vector3 center)
        {
            Camara.position = position;
            Camara.center = center;

            camaraRadiusChanged = true;

            Look();
            UpdateDirVectors();
            CalcYaw();
            CalcPitch();
        }

        static public void LocateCamara(float[] parameters)
        {

            Camara.position = new Vector3(parameters[0], parameters[1], parameters[2]);
            Camara.center = new Vector3(parameters[3], parameters[4], parameters[5]);

            camaraRadiusChanged = true;

            Look();
            UpdateDirVectors();
            CalcYaw();
            CalcPitch();
        }

        void CalcCamaraRadius()
        {
            double difX = position.X - center.X;
            double difY = position.Z - center.Z;
            camaraRadius = Math.Sqrt(Math.Pow(difX, 2) + Math.Pow(difY, 2));
        }
        
        void OrbitRotation(float angle)
        {
            double difX = position.X - center.X;
            double difY = position.Z - center.Z;
            yaw -= angle;
            if (yaw > 360)
            {
                yaw -= 360;
            }
            else
            {
                if (yaw < -360)
                {
                    yaw += 360; 
                }
            }
            if (camaraRadiusChanged)//calc the camera radius if this has changed
            {
                camaraRadius = Math.Sqrt(Math.Pow(difX, 2) + Math.Pow(difY, 2));
                camaraRadiusChanged = false;
            }
             
            
            difX -= camaraRadius * Math.Sin(Helper.DegreeToRad(-(double)yaw));
            difY -= camaraRadius * Math.Cos(Helper.DegreeToRad(-(double)yaw));
            
            position.X -= (float)difX;
            position.Z -= (float)difY; 
            
            rotating = true; 
            if (camaraDirection == 1 || camaraDirection == -1)
            {
                //Sound.PlayLoop("cameraRotate.wav");  
            }
            if (camaraDirection < 0)
            {
                camaraDirection--;
            }
            else
            {
                camaraDirection++;
            }
        }

        static void CalcPitch()
        { 
            pitch = Helper.RadToDegree(Math.Asin(Math.Abs(j))); 
        }

        static void CalcYaw()
        {
            yaw = -Helper.RadToDegree(Math.Atan2(Math.Abs(i), Math.Abs(k)));
            if (i > 0 && k < 0)
            {
                yaw *= -1; //cuadrante 3
            }
            if (i > 0 && k > 0)
            {
                yaw -= 180; //cuadrante 2
            }
            if (i < 0 && k > 0)
            {
                yaw = -(180 + yaw); //cuadrante 1 (angulo complementario)
            }
        }

        static void UpdateDirVectors()
        {
            forwardVector = (position - center);
            forwardVector.Normalize();  
            
            i = -forwardVector.X;
            j = -forwardVector.Y;
            k = -forwardVector.Z;
        }

        void MoveCam(int direction)
        {
            float speeedScrooll = 1;
            if (scrolling)
            {
                speeedScrooll += scrollSpeed;
                scrolling = false; 
            }
            if (camaraRadius < 30 || direction == 1)//para que no se aleje  mucho la camara
            {
                if (position.Y > 0.1f)// para que no se hunda
                {
                    position.X += (float)i * forwardSpeed * direction * speeedScrooll;
                    position.Y += (float)j * forwardSpeed * direction * speeedScrooll;
                    position.Z += (float)k * forwardSpeed * direction * speeedScrooll;
                    CalcCamaraRadius(); 
                }
            }           
        }

        void UpDown(int direction)
        {
            if (position.Y + (float)j * forwardSpeed * direction > 0.5f)
            {
                position.Y += (float)j * forwardSpeed * direction; 
            }
            
            UpdateDirVectors();
            CalcPitch(); 
        }

        public void Update()
        {
            ShakeCamera();

            if (!locked && !lockedTop && !lockedMisileCamera)
            {
                #region Camara de juego

                movement = Vector3.Zero;
                MouseState m = Mouse.GetState();
                KeyboardState k = Keyboard.GetState();   

                switch (move)
                {
                    case CameraMove.Left:
                        if (!rotating)
                            camaraDirection = -1;

                        OrbitRotation(-rotationSpeed); //A
                        break;
                    case CameraMove.Right:
                        if (!rotating)
                        {
                            camaraDirection = 1;
                        }
                        OrbitRotation(rotationSpeed);//D
                        break;
                    case CameraMove.Up:
                        UpDown(-2);
                        break;
                    case CameraMove.Down:
                        UpDown(2);
                        break;
                    case CameraMove.ZoomIn:
                        rotating = false;
                        MoveCam(1);
                        break;
                    case CameraMove.ZoomOut:
                        rotating = false;
                        MoveCam(-1);
                        break;
                    default:
                        break;
                }

                move = CameraMove.None;
                UpdateDirVectors();


                #endregion

                #region Movimiento por el teclado

                if (k.IsKeyDown(Keys.W))
                    UpDown(-2);

                if (k.IsKeyDown(Keys.S))
                    UpDown(2);

                #endregion
            }

            if (!locked && !lockedMisileCamera)
            {
                Look();
            }
        }

        public void Reset()
        {
            lockedTop = false;
            locked = false;
            lockedMisile = false;
        }
    }
}
