public class Action : ActionFixture {
public override bool check(){
Accumulator acc = new Accumulator();
acc.add(product(12,12));
return result(sqrt(acc.add(product(7,7))));
}
public bool result(object o){return o.Equals(13.8924);}
}

