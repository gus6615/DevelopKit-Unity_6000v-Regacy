using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesignPattern.CreatePattern.FactoryMethod
{
    public abstract class UnitGenerator
    {
        protected List<Unit> units = new List<Unit>();
        public List<Unit> Units => units;
        public abstract void CreateUnit();
    }
    
    public class Generate_A : UnitGenerator
    {
        public override void CreateUnit()
        {
            units.Add(new Marine());
            units.Add(new Marine());
            units.Add(new Marine());
            units.Add(new Marine());
            units.Add(new Marine());
            units.Add(new Marine());
            units.Add(new Marine());
            units.Add(new Marine());
        }
    }
    
    public class Generate_B : UnitGenerator
    {
        public override void CreateUnit()
        {
            units.Add(new Marine());
            units.Add(new Marine());
            units.Add(new Marine());
            units.Add(new Firebat());
            units.Add(new Firebat());
            units.Add(new Firebat());
            units.Add(new Firebat());
            units.Add(new Firebat());
        }
    }
}
