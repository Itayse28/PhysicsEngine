using Microsoft.VisualBasic.Devices;
using System.CodeDom;
using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace GraphicsTest
{
    public partial class Form1 : Form
    {

        Scene scene;
        public Form1()
        {
            InitializeComponent();
            scene = new Scene(this);

            this.MouseWheel += Form1_MouseWheel;
        }
        public void preset()
        {
            scene.addPhysicsObject("sphere", -300, 0, 200, true, false, 5, 0, -7, 2, GravityBar.Value);
            scene.addPhysicsObject("sphere", 300, 0, 200, false, false, -10, 0, -4, 2, GravityBar.Value);
            int countObjects = 2;
            for (int i = -1; i < 1; i++)
                for (int j = -1; j < 1; j++)
                {
                    scene.addPhysicsObject("sphere", i * 150, -100, 150 * j, false, false, 0, 0, 0, 1, GravityBar.Value);
                    countObjects++;
                }
            for (int i = -4; i < 5; i++)
                for (int j = -5; j < 5; j++)
                {
                    scene.addPhysicsObject("cube", 100 * i, 400, 100 * j, false, false, 0, 0, 0);
                    ((PhysicsObject)scene.objects[countObjects]).yforce = 0;
                    countObjects++;
                }
            for (int i = -5; i < 4; i++)
                for (int j = -5; j < 5; j++)
                {
                    scene.addPhysicsObject("cube", -500, i * 100, 100 * j, false, false, 0, 0, 0);
                    ((PhysicsObject)scene.objects[countObjects]).yforce = 0;
                    countObjects++;
                }
            for (int i = -5; i < 4; i++)
                for (int j = -5; j < 5; j++)
                {
                    scene.addPhysicsObject("cube", 500, i * 100, 100 * j, false, false, 0, 0, 0);
                    ((PhysicsObject)scene.objects[countObjects]).yforce = 0;
                    countObjects++;
                }
            for (int i = -5; i < 5; i++)
                for (int j = -4; j < 5; j++)
                {
                    scene.addPhysicsObject("cube", 100 * j, i * 100, 500, false, false, 0, 0, 0);
                    ((PhysicsObject)scene.objects[countObjects]).yforce = 0;
                    countObjects++;
                }
            for (int i = -4; i < 5; i++)
                for (int j = -5; j < 4; j++)
                {
                    scene.addPhysicsObject("cube", 100 * i, 100 * j, -600, false, false, 0, 0, 0);
                    ((PhysicsObject)scene.objects[countObjects]).yforce = 0;
                    countObjects++;
                }

        }
        private void RenderLoop_Tick(object sender, EventArgs e)
        {
            int start = DateTime.Now.Millisecond;
            
            string shortx= scene.cam.x.ToString(), shorty= scene.cam.y.ToString(), shortz= scene.cam.z.ToString();
            if (scene.cam.x != (int)scene.cam.x)
                shortx = shortx.Substring(0, shortx.IndexOf('.') + 3);
            if (scene.cam.y != (int)scene.cam.y)
                shorty = shorty.Substring(0, shorty.IndexOf('.') + 3);
            if (scene.cam.z != Math.Round(scene.cam.z))
                shortz = shortz.Substring(0, shortz.IndexOf('.') + 3);
            label1.Text = shortx + ", " + shorty + ", " + shortz;
            
            scene.update();
            label2.Text = "Calc: " + scene.calcT + " Draw: " + scene.drawT + " Render: " + scene.renderT;

            int end = DateTime.Now.Millisecond;
            if (end == start)
                return;

            FPS.Text = "FPS: "+(1000/(end-start));
            

        }


        private void Form1_Resize(object sender, EventArgs e)
        {
            scene.formResize(this);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            scene.keyDown(e);
            if (e.KeyCode == Keys.Enter)
                if (Item.SelectedItem != null)
                    scene.addObject(Item.SelectedItem.ToString(), scene.cam.x, scene.cam.y, scene.cam.z,doRotate.Checked,showFaces.Checked);
        }

        private void addModel_Click(object sender, EventArgs e)
        {
            if (Item.SelectedItem != null)
            {
                if (Physics.Checked)
                {
                    if (PassiveCheckBox.Checked)
                    {
                        scene.addPhysicsObject(Item.SelectedItem.ToString(), scene.cam.x, scene.cam.y, scene.cam.z, doRotate.Checked, showFaces.Checked,
                            (double)physicsSpeedX.Value, (double)physicsSpeedY.Value, (double)physicsSpeedZ.Value);
                        ((PhysicsObject)scene.objects[scene.objects.Count - 1]).yforce = 0;
                    }
                    else
                        scene.addPhysicsObject(Item.SelectedItem.ToString(), scene.cam.x, scene.cam.y, scene.cam.z, doRotate.Checked, showFaces.Checked,
                            (double)physicsSpeedX.Value, (double)physicsSpeedY.Value, (double)physicsSpeedZ.Value, 2, GravityBar.Value);
                }
                else
                    scene.addObject(Item.SelectedItem.ToString(), scene.cam.x, scene.cam.y, scene.cam.z, doRotate.Checked, showFaces.Checked);
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            scene.mouseMove(e);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown nud = (NumericUpDown)sender;
            scene.renderDistance = nud.Value;
        }
        private void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            scene.step += e.Delta/120;
            if(scene.step < 1)
                scene.step = 1;
        }

        private void Physics_CheckedChanged(object sender, EventArgs e)
        {
            if (Physics.Checked)
            {
                physicsSpeedX.Visible = true;
                physicsSpeedY.Visible = true;
                physicsSpeedZ.Visible = true;
                speedLabel.Visible = true;
                PassiveCheckBox.Visible = true;
            }
            else
            {
                physicsSpeedX.Visible = false;
                physicsSpeedY.Visible = false;
                physicsSpeedZ.Visible = false;
                speedLabel.Visible = false;
                PassiveCheckBox.Visible = false;
            }
        }

        private void PresetButton_Click(object sender, EventArgs e)
        {
            this.preset();
            PresetButton.Visible = false;
            PresetButton.Enabled = false;
        }

        private void GravityBar_Scroll(object sender, EventArgs e)
        {
            for (int i = 0; i < scene.objects.Count; i++)
                if (scene.objects[i] is PhysicsObject && ((PhysicsObject)scene.objects[i]).weight < 2000000)
                    ((PhysicsObject)scene.objects[i]).yforce = 0.2 * GravityBar.Value;
        }
    }
}

/*  5.10.2024
 * 
 * 
 * Y+ is down
 * X+ is right
 * Z+ is forward
 * -------->x
 * |⟍
 * |  ⟍
 * |    ⟍
 * V      🡮
 * Y        Z
 * 
 * 
 * 
 * 
 *  this is an extended version to a graphics engine that i wrote in 2021
 *  the physics here are very basic because i coded it in 3 hours at a tuesday night
 * 
 * 
 * 
 */












