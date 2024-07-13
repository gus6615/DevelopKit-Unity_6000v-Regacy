using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesignPattern.CreatePattern.FactoryMethod
{
    public abstract class Unit
    {
        protected string name;
        protected int hp;
        protected int damage;
    }
    
    public class Marine : Unit
    {
        public Marine()
        {
            name = "Marine";
            hp = 40;
            damage = 6;
        }
    }
    
    public class Firebat : Unit
    {
        public Firebat()
        {
            name = "Firebat";
            hp = 50;
            damage = 8;
        }
    }
}