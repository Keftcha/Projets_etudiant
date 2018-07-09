using System;

using NUnit.Framework;

namespace MiamiOps.Tests
{

    [TestFixture]
    public class MiamiOpsColsionTests
    {
        [TestCase(0, 1, 0.0, .1f, .2f, .4f, 1.0)]
        [TestCase(0, -1, 0.0, -.1f, -.2f, -.4f, -0.8)]
        public void Look_if_the_player_move_verticaly(float xDirection, float yDirection, double t0, double t1, double t2, double t3, double t4)
        {
            Round play = new Round(0, new Vector(0, 0), playerLargeur : 0.1f, playerHauteur : 0.2f);
            Assert.That(Math.Round(play.Player.Place.Y, 2), Is.EqualTo(Math.Round(t0, 2)));
            play.Player.Move(new Vector(xDirection, yDirection));
            Assert.That(Math.Round(play.Player.Place.Y, 2), Is.EqualTo(Math.Round(t1, 2)));
            play.Player.Move(new Vector(xDirection, yDirection));
            Assert.That(Math.Round(play.Player.Place.Y, 2), Is.EqualTo(Math.Round(t2, 2)));
            play.Player.Move(new Vector(xDirection, yDirection));
            play.Player.Move(new Vector(xDirection, yDirection));
            Assert.That(Math.Round(play.Player.Place.Y, 2), Is.EqualTo(Math.Round(t3, 2)));
            for (int idx = 0; idx < 10; idx += 1)
            {
                play.Player.Move(new Vector(xDirection, yDirection));
            }
            Assert.That(Math.Round(play.Player.Place.Y, 2), Is.EqualTo(Math.Round(t4, 2)));
        }

        [TestCase(-1, 0, -0.0, -.1f, -.2f, -.4f, -1.0)]
        [TestCase(1, 0, 0.0, .1f, .2f, .4f, .9f)]
        public void Look_if_the_player_move_horizontaly(float xDirection, float yDirection, double t0, double t1, double t2, double t3, double t4)
        {
            Round play = new Round(0, new Vector(0, 0), playerLargeur : 0.1f, playerHauteur : 0.2f);
            Assert.That(Math.Round(play.Player.Place.X, 2), Is.EqualTo(Math.Round(t0, 2)));
            play.Player.Move(new Vector(xDirection, yDirection));
            Assert.That(Math.Round(play.Player.Place.X, 2), Is.EqualTo(Math.Round(t1, 2)));
            play.Player.Move(new Vector(xDirection, yDirection));
            Assert.That(Math.Round(play.Player.Place.X, 2), Is.EqualTo(Math.Round(t2, 2)));
            play.Player.Move(new Vector(xDirection, yDirection));
            play.Player.Move(new Vector(xDirection, yDirection));
            Assert.That(Math.Round(play.Player.Place.X, 2), Is.EqualTo(Math.Round(t3, 2)));
            for (int idx = 0; idx < 6; idx += 1)
            {
                play.Player.Move(new Vector(xDirection, yDirection));
            }
            Assert.That(Math.Round(play.Player.Place.X, 2), Is.EqualTo(Math.Round(t4, 2)));
        }

        [TestCase(1, -1, 0, 2f, 2f, -1, .2f)]
        [TestCase(-.6f, -1, 1, 2f, 1f, 1, 0f)]
        [Explicit("Player.Move() must be fixed.")]
        public void The_player_can_not_go_in_a_wall_verticaly(float spawn_y, float x_obstacle, float y_obstacle, float largeur_obstacle, float hauteur_obstacle, float direction_y, float arrive_y)
        {
            Round play = new Round(0, new Vector(0, spawn_y), playerLargeur : 0.1f, playerHauteur : 0.2f);
            play.AddObstacle(x_obstacle, y_obstacle, largeur_obstacle, hauteur_obstacle);
            for (int idx = 0; idx < 15; idx += 1) {play.Player.Move(new Vector (0, direction_y));}
            Assert.That(Math.Round(play.Player.Place.Y, 2), Is.EqualTo(Math.Round(arrive_y, 2)));
        }

        [TestCase(-1f, 0, 1, 1, 2, 1, -.1f)]
        [TestCase(1f, -1, 1, 1, 2, -1f, 0f)]
        [Explicit("Player.Move() must be fixed.")]
        public void The_player_can_not_go_in_a_wall_horizontaly(float spawn_x, float x_obstacle, float y_obstacle, float largeur_obstacle, float hauteur_obstacle, float direction_x, float arrive_x)
        {
             Round play = new Round(0, new Vector(spawn_x, 0), playerLargeur : 0.1f, playerHauteur : 0.2f);
             play.AddObstacle(x_obstacle, y_obstacle, largeur_obstacle, hauteur_obstacle);
             for (int idx = 0; idx < 15; idx += 1) {play.Player.Move(new Vector (direction_x, 0));}
             Assert.That(Math.Round(play.Player.Place.X, 2), Is.EqualTo(Math.Round(arrive_x, 2)));
        }

        [TestCase(1, -1, -1, 0, 2f, 2f, -1, .2f)]
        [TestCase(-.6f, 1, -1, 1, 2f, 1f, 1, 0f)]
        public void Enemie_can_not_go_in_a_wall_verticaly(float spawn_y, float player_place_y, float x_obstacle, float y_obstacle, float largeur_obstacle, float hauteur_obstacle, float direction_y, float arrive_y)
        {
            Round play = new Round(1, new Vector(0, player_place_y), new Vector(0, spawn_y), enemiesHauteur : 0.2f, enemiesLargeur : 0.1f, enemiesSpeed : .1f);
            play.AddObstacle(x_obstacle, y_obstacle, largeur_obstacle, hauteur_obstacle);
            for (int idx = 0; idx < 15; idx += 1) {play.Update();}
            Assert.That(Math.Round(play.Enemies[0].Place.Y, 2), Is.EqualTo(Math.Round(arrive_y, 2)));
        }

        [TestCase(1f, -1, -1, 1, 1, 2f, 0f)]
        [TestCase(-1, 1, 0, 1, 2f, 2f, -.1f)]
        public void Enemie_can_not_go_in_a_wall_horizontaly(float spawn_x, float player_place_x, float x_obstacle, float y_obstacle, float largeur_obstacle, float hauteur_obstacle, float arrive_x)
        {
            Round play = new Round(1, playerSpawn: new Vector(player_place_x, 0), enemieSpawn: new Vector(spawn_x, 0), enemiesHauteur : 0.2f, enemiesLargeur : 0.1f, enemiesSpeed : .1f);
            play.AddObstacle(x_obstacle, y_obstacle, largeur_obstacle, hauteur_obstacle);
            for (int idx = 0; idx < 15; idx += 1) {play.Update();}
            Assert.AreEqual(arrive_x, play.Enemies[0].Place.X, 0.01);
        }

        [Test]
        public void Test_if_triangle_colide()
        {
            bool colide;
            colide = ColideHelpers.areColide(new(double, double)[3] { (1, 3),(3, 3),(5, 5) },new(double, double)[3] { (2, 2),(4, 2),(4, 5) });
            Assert.That(colide,Is.True);
            colide = ColideHelpers.areColide(new(double, double)[3] { (2, 1),(4, 3),(4, 1) },new(double, double)[3] { (4, 0),(3, 3),(5, 2) });
            Assert.That(colide,Is.True);
            colide = ColideHelpers.areColide(new(double, double)[3] { (4, 0),(3, 3),(5, 2) },new(double, double)[3] { (2, 1),(4, 3),(4, 1) });
            Assert.That(colide,Is.True);
            colide = ColideHelpers.areColide(new(double, double)[3] { (1, 1),(3, 6),(5, 1) },new(double, double)[3] { (2, 2),(3, 4),(4, 2) });
            Assert.That(colide,Is.True);
            colide = ColideHelpers.areColide(new(double, double)[3] { (1, 4),(2, 6),(3, 4) },new(double, double)[3] { (2, 2),(6, 5),(5, 3) });
            Assert.That(colide,Is.False);
            colide = ColideHelpers.areColide(new(double, double)[3] { (2, 2),(6, 5),(5, 3) },new(double, double)[3] { (1, 4),(2, 6),(3, 4) });
            Assert.That(colide,Is.False);
            colide = ColideHelpers.areColide(new(double, double)[3] { (1, 0),(2, 0),(3, 0) },new(double, double)[3] { (2, 3),(6, 3),(5, 3) });
            Assert.That(colide,Is.False);
            colide = ColideHelpers.areColide(new(double, double)[3] { (0, 0),(0, 0),(0, 0) },new(double, double)[3] { (2, 2),(6, 5),(5, 3) });
            Assert.That(colide,Is.False);
            colide = ColideHelpers.areColide(new(double, double)[3] { (-1, 0),(-.8, 0),(-1, -0.2) },new(double, double)[3] { (0, 1),(1, 1),(0, -1) });
            Assert.That(colide,Is.False);
        }
    }
}
