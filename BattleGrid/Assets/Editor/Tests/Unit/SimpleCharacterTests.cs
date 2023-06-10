using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JoeKyuu.BattleGrid.Tests.Unit.SimpleCharacterTests
{
    [TestFixture]
    public class SimpleCharacter_Logic_Tests
    {
        const string NAME_FORMAT = @"Test_{0}";
        const int HEALTH_DEFAULT = 100;
        const int DEFENSE_DEFAULT = 10;
        const int ATTACK_DEFAULT = 20;

        SimpleCharacter CreateCharacter(int health, int defense, int attack, string nameFragment)
        {
            SimpleCharacterFacade facade = new SimpleCharacterFacade { Name = string.Format(NAME_FORMAT, nameFragment), Attack = attack, Defense = defense, Health = health };
            return new SimpleCharacter(facade);
        }

        [Test]
        public void Create_Valid_Character()
        {
            var simpleCharacter = CreateCharacter(HEALTH_DEFAULT, DEFENSE_DEFAULT, ATTACK_DEFAULT,"A");
            Assert.IsNotNull(simpleCharacter);
        }

        [Test]
        public void Can_Set_Status()
        {
            var simpleCharacter = CreateCharacter(HEALTH_DEFAULT, DEFENSE_DEFAULT, ATTACK_DEFAULT, "A");
            simpleCharacter.SetStatus(SimpleCharacterStatus.WEAKENED);
            Debug.Log($"{simpleCharacter.Status.ToString()}");
            Assert.IsTrue(simpleCharacter.Status.HasFlag(SimpleCharacterStatus.WEAKENED));
        }

        [Test]
        public void Can_Set_Multiple_Status()
        {
            var simpleCharacter = CreateCharacter(HEALTH_DEFAULT, DEFENSE_DEFAULT, ATTACK_DEFAULT, "A");
            SimpleCharacterStatus status = (SimpleCharacterStatus.INVIGORATED | SimpleCharacterStatus.SWAGGERRING);
            Debug.Log($"{simpleCharacter.Status.ToString()}  {status.ToString()}");
            simpleCharacter.SetStatus(status);
            Debug.Log($"{simpleCharacter.Status.ToString()}  {status.ToString()}");
            var hasFlagsForInvigoratedAndSwaggering = ((simpleCharacter.Status & (SimpleCharacterStatus.INVIGORATED | SimpleCharacterStatus.SWAGGERRING)) != 0);
            Assert.IsTrue(hasFlagsForInvigoratedAndSwaggering);
        }

        [Test]
        public void Can_UnSet_Status()
        {
            var simpleCharacter = CreateCharacter(HEALTH_DEFAULT, DEFENSE_DEFAULT, ATTACK_DEFAULT, "A");
            SimpleCharacterStatus status = (SimpleCharacterStatus.INVIGORATED | SimpleCharacterStatus.SWAGGERRING);
            simpleCharacter.SetStatus(status);
            simpleCharacter.UnSetStatus(SimpleCharacterStatus.SWAGGERRING);
            Assert.IsTrue(simpleCharacter.Status.HasFlag(SimpleCharacterStatus.INVIGORATED));
            Assert.IsFalse(simpleCharacter.Status.HasFlag(SimpleCharacterStatus.SWAGGERRING));
        }

        [Test]
        public void On_Death_UnSet_Status()
        {
            var simpleCharacter = CreateCharacter(HEALTH_DEFAULT, DEFENSE_DEFAULT, ATTACK_DEFAULT, "A");
            SimpleCharacterStatus status = (SimpleCharacterStatus.INVIGORATED | SimpleCharacterStatus.SWAGGERRING | SimpleCharacterStatus.WEAKENED | SimpleCharacterStatus.CRAZED);
            simpleCharacter.SetStatus(status);
            var hasFlags = ((simpleCharacter.Status & (SimpleCharacterStatus.INVIGORATED | SimpleCharacterStatus.SWAGGERRING | SimpleCharacterStatus.WEAKENED | SimpleCharacterStatus.CRAZED)) != 0);
            Assert.IsTrue(hasFlags);
            simpleCharacter.SetStatus(SimpleCharacterStatus.DEAD);
            hasFlags = ((simpleCharacter.Status & (SimpleCharacterStatus.INVIGORATED | SimpleCharacterStatus.SWAGGERRING | SimpleCharacterStatus.WEAKENED | SimpleCharacterStatus.CRAZED)) != 0);
            Assert.IsFalse(hasFlags);
            Assert.IsTrue(((simpleCharacter.Status & SimpleCharacterStatus.DEAD) != 0));
        }
    }
}
