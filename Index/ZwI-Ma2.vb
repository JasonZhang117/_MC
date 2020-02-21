//Zw-I-Ma2
inputs: Ma1Length(30); //短均线bar数量
inputs: Ma2Length(120); //长均线bar数量
variables: Ma1(Close); //短均线值
variables: Ma2(Close); //长均线值

value1 = ZwF_Ma2(Ma1Length,Ma2Length,Ma1,Ma2);

Plot1(Ma1,"Ma1");
Plot2(Ma2,"Ma2");
