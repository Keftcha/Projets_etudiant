using System;
using System.Collections.Generic;

using NUnit.Framework;


namespace MiamiOps.Tests
{
    [TestFixture]
    public class MiamiOpsTests
    {
        [TestCase(-4, 0, 0, 0, 0, 0)]
        [TestCase(0, -4, 0, 0, 0, 0)]
        [TestCase(0, 0, -4, 0, 0, 0)]
        [TestCase(0, 0, 0, -4, 0, 0)]
        [TestCase(0, 0, 0, 0, -4, 0)]
        [TestCase(0, 0, 0, 0, 0, -4)]
        [TestCase(0, 4, 0, 0, 0, 0)]
        [TestCase(0, 0, 4, 0, 0, 0)]
        [TestCase(0, 0, 0, 4, 0, 0)]
        [TestCase(0, 0, 0, 0, 4, 0)]
        [TestCase(0, 0, 0, 0, 0, 4)]
        public void Create_round_check_how_may_enemies_is_in_the_round(int nb_of_enemies, float enemies_life, float enemies_speed, float enemies_attack, float player_life, float player_speed)
        {
            Assert.Throws<ArgumentException>( () => new Round(nb_of_enemies, new Vector(0f, 0f), new Vector(0f, 0f), enemies_life, enemies_speed, enemies_attack, player_life, player_speed));
            Assert.Throws<ArgumentException>( () => new Round(nb_of_enemies, new Vector(0f, 0f), enemiesLife: enemies_life, enemiesSpeed: enemies_speed, enemiesAttack: enemies_attack, playerLife: player_life, playerSpeed: player_speed));
            Assert.Throws<ArgumentException>( () => new Round(nb_of_enemies, enemiesLife: enemies_life, enemiesSpeed: enemies_speed, enemiesAttack: enemies_attack, playerLife: player_life, playerSpeed: player_speed));
        }

        [Test]
        public void Create_round_with_enemies_spawn_out_of_the_map()
        {
            (int, int)[] vectors = new (int, int)[4]{(0, 2), (0, -2), (2, 0), (-2, 0)};
            foreach ((int, int) spawnLocation in vectors) Assert.Throws<ArgumentException>( () => new Round(10, new Vector(0f, 0f), new Vector(spawnLocation.Item1, spawnLocation.Item2)));
        }

        [Test]
        public void Create_round_with_player_place_out_of_map()
        {
            (int, int)[] vectors = new (int, int)[4]{(0, 2), (0, -2), (2, 0), (-2, 0)};
            foreach ((int, int) spawnLocation in vectors) Assert.Throws<ArgumentException>( () => new Round(10, new Vector(spawnLocation.Item1, spawnLocation.Item2), new Vector(0, 0)));
            foreach ((int, int) spawnLocation in vectors) Assert.Throws<ArgumentException>( () => new Round(10, new Vector(spawnLocation.Item1, spawnLocation.Item2)));
        }

        [Test]
        public void Look_if_enemies_go_to_the_player__go_down()
        {
            Round play = new Round(1, new Vector(0, 0), new Vector(0, 1));
            Assert.That(Math.Round(play.Enemies[0].Place.Y, 2), Is.EqualTo(Math.Round(1.0, 2)));
            play.Update();
            Assert.That(Math.Round(play.Enemies[0].Place.Y, 2), Is.EqualTo(Math.Round(.95, 2)));
            play.Update();
            Assert.That(Math.Round(play.Enemies[0].Place.Y, 2), Is.EqualTo(Math.Round(.90f, 2)));
            play.Update();
            play.Update();
            Assert.That(Math.Round(play.Enemies[0].Place.Y, 2), Is.EqualTo(Math.Round(.80f, 2)));
            for (int idx = 0; idx < 8; idx += 1)
            {
                play.Update();
                play.Update();
            }
            Assert.That(Math.Round(play.Enemies[0].Place.Y, 2), Is.EqualTo(Math.Round(0.0, 2)));
        }

        [Test]
        public void Look_if_enemies_go_to_the_player__go_up()
        {
            Round play = new Round(1, new Vector(0, 0), new Vector(0, -1));
            Assert.That(Math.Round(play.Enemies[0].Place.Y, 2), Is.EqualTo(Math.Round(-1.0, 2)));
            play.Update();
            Assert.That(Math.Round(play.Enemies[0].Place.Y, 2), Is.EqualTo(Math.Round(-.95, 2)));
            play.Update();
            Assert.That(Math.Round(play.Enemies[0].Place.Y, 2), Is.EqualTo(Math.Round(-.90f, 2)));
            play.Update();
            play.Update();
            Assert.That(Math.Round(play.Enemies[0].Place.Y, 2), Is.EqualTo(Math.Round(-.80f, 2)));
            for (int idx = 0; idx < 8; idx += 1)
            {
                play.Update();
                play.Update();
            }
            Assert.That(Math.Round(play.Enemies[0].Place.Y, 2), Is.EqualTo(Math.Round(-0.0, 2)));
        }

        [Test]
        public void Look_if_enemies_go_to_the_player__go_left()
        {
            Round play = new Round(1, new Vector(0, 0), new Vector(1, 0));
            Assert.That(Math.Round(play.Enemies[0].Place.X, 2), Is.EqualTo(Math.Round(1.0, 2)));
            play.Update();
            Assert.That(Math.Round(play.Enemies[0].Place.X, 2), Is.EqualTo(Math.Round(.95, 2)));
            play.Update();
            Assert.That(Math.Round(play.Enemies[0].Place.X, 2), Is.EqualTo(Math.Round(.90f, 2)));
            play.Update();
            play.Update();
            Assert.That(Math.Round(play.Enemies[0].Place.X, 2), Is.EqualTo(Math.Round(.80f, 2)));
            for (int idx = 0; idx < 8; idx += 1)
            {
                play.Update();
                play.Update();
            }
            Assert.That(Math.Round(play.Enemies[0].Place.X, 2), Is.EqualTo(Math.Round(0.0, 2)));
        }

        [Test]
        public void Look_if_enemies_go_to_the_player__go_rigth()
        {
            Round play = new Round(1, new Vector(0, 0), new Vector(-1, 0));
            Assert.That(Math.Round(play.Enemies[0].Place.X, 2), Is.EqualTo(Math.Round(-1.0, 2)));
            play.Update();
            Assert.That(Math.Round(play.Enemies[0].Place.X, 2), Is.EqualTo(Math.Round(-.95, 2)));
            play.Update();
            Assert.That(Math.Round(play.Enemies[0].Place.X, 2), Is.EqualTo(Math.Round(-.90f, 2)));
            play.Update();
            play.Update();
            Assert.That(Math.Round(play.Enemies[0].Place.X, 2), Is.EqualTo(Math.Round(-.80f, 2)));
            for (int idx = 0; idx < 8; idx += 1)
            {
                play.Update();
                play.Update();
            }
            Assert.That(Math.Round(play.Enemies[0].Place.X, 2), Is.EqualTo(Math.Round(-0.0, 2)));
        }

        [TestCase(0, 1, 0.0, .1f, .2f, .4f, 1.0)]
        [TestCase(0, -1, 0.0, -.1f, -.2f, -.4f, -1)]
        public void Look_if_the_player_move_verticaly(float xDirection, float yDirection, double t0, double t1, double t2, double t3, double t4)
        {
            Round play = new Round(0, new Vector(0, 0));
            Assert.That(Math.Round(play.Player.Place.Y, 2), Is.EqualTo(Math.Round(t0, 2)));
            play.Player.Move(new Vector(xDirection, yDirection));
            Assert.That(Math.Round(play.Player.Place.Y, 2), Is.EqualTo(Math.Round(t1, 2)));
            play.Player.Move(new Vector(xDirection, yDirection));
            Assert.That(Math.Round(play.Player.Place.Y, 2), Is.EqualTo(Math.Round(t2, 2)));
            play.Player.Move(new Vector(xDirection, yDirection));
            play.Player.Move(new Vector(xDirection, yDirection));
            Assert.That(Math.Round(play.Player.Place.Y, 2), Is.EqualTo(Math.Round(t3, 2)));
            for (int idx = 0; idx < 6; idx += 1)
            {
                play.Player.Move(new Vector(xDirection, yDirection));
            }
            Assert.That(Math.Round(play.Player.Place.Y, 2), Is.EqualTo(Math.Round(t4, 2)));
        }

        [TestCase(-1, 0, -0.0, -.1f, -.2f, -.4f, -1)]
        [TestCase(1, 0, 0.0, .1f, .2f, .4f, 1)]
        public void Look_if_the_player_move_horizontaly(float xDirection, float yDirection, double t0, double t1, double t2, double t3, double t4)
        {
            Round play = new Round(0, new Vector(0, 0));
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

        [TestCase(0, 1, 0, 1)]    // In the top
        [TestCase(0, -1, 0, -1)]    // In the bottom
        [TestCase(-1, 0, -1, 0)]    // In the left
        [TestCase(1, 0, 1, 0)]    // In the rigth
        public void The_player_can_not_go_out_of_the_map(float xBegin, float yBegin, float xDirection, float yDirection)
        {
            Round play = new Round(0, new Vector(xBegin, yBegin));
            play.Player.Move(new Vector(xDirection, yDirection));
            play.Update();
            Assert.That(Math.Round(play.Player.Place.X, 2), Is.EqualTo(Math.Round(xBegin, 2)));
            Assert.That(Math.Round(play.Player.Place.Y, 2), Is.EqualTo(Math.Round(yBegin, 2)));
        }

        [Test]
        public void When_create_Round_with_no_enemiesSpawn_the_enemies_are_not_at_the_same_place()
        {
            Round play = new Round(2);
            bool sameX = Math.Round(play.Enemies[0].Place.X, 2) == Math.Round(play.Enemies[1].Place.X, 2);
            bool sameY = Math.Round(play.Enemies[0].Place.Y, 2) == Math.Round(play.Enemies[1].Place.Y, 2);
            bool samePlace = sameX && sameY;
            Assert.That(samePlace, Is.Not.EqualTo(true));
        }
    }
}