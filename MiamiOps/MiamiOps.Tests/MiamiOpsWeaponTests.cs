using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;


namespace MiamiOps.Tests
{
    [TestFixture]
    class MiamiOpsWeaponTest
    {
        [TestCase("C", 1, "D")]
        [TestCase("C", -1, "B")]
        [TestCase("A", -1, "F")]
        [TestCase("F", 1, "A")]
        [TestCase("C", -4, "E")]
        [TestCase("E", 3, "B")]
        public void Test_the_list_helpers__the_change_element_class_method(string current, int shift, string expected)
        {
            List<string> letters = new List<string>{"A", "B", "C", "D", "E", "F"};
            string letter = letters.OtherElem(current, shift);
            Assert.That(letter, Is.EqualTo(expected));
        }

        [Test]
        public void The_player_change_his_weapon()
        {
            WeaponFactory factory = new WeaponFactory();
            Player player = CreatePlayer(factory);

            Assert.That(player.CurrentWeapon, Is.EqualTo(player.Weapons[2]));
            player.ChangeWeapon(1);
            Assert.That(player.CurrentWeapon, Is.EqualTo(player.Weapons[0]));
            player.ChangeWeapon(-2);
            player.ChangeWeapon(2);
            Assert.That(player.CurrentWeapon, Is.EqualTo(player.Weapons[0]));
        }

        [Test]
        public void If_the_player_has_already_a_weapon_and_he_get_a_new_one_his_new_weapon_is_in_his_hand()
        {
            WeaponFactory factory = new WeaponFactory();
            Player player = CreatePlayer(factory);

            Assert.That(player.CurrentWeapon, Is.EqualTo(player.Weapons[2]));
            player.GetNewWeapon(factory.CreateShotgun(player));
            Assert.That(player.CurrentWeapon, Is.EqualTo(player.Weapons[3]));
        }

        [Test]
        public void If_the_player_have_any_weapon_nothing_append_when_he_try_to_change()
        {
            Player player = new Player(null, new Vector(0, 0), 1, 1, new Vector(0, 0));
            player.ChangeWeapon(9);
            Assert.That(player.CurrentWeapon, Is.EqualTo(null));
        }

        [Test]
        public void If_the_player_get_a_weapon__the_first__the_weapon_is_in_his_hand()
        {
            WeaponFactory factory = new WeaponFactory();
            Player player = new Player(null, new Vector(0, 0), 1, 1);
            player.GetNewWeapon(factory.CreateAssaultRifle(player));
            Assert.That(player.CurrentWeapon, Is.EqualTo(player.Weapons[0]));
        }

        private Player CreatePlayer(WeaponFactory factory)
        {
            Player player = new Player(new List<Weapon>(), null, new Vector(0, 0), 1, 1, new Vector(0, 0));
            player.GetNewWeapon(factory.CreateAssaultRifle(player));
            player.GetNewWeapon(factory.CreateBaseballBat(player));
            player.GetNewWeapon(factory.CreateSoulcalibur(player));

            return player;
        }
    }
}
