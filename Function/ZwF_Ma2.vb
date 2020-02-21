//Zw_F_Ma2
inputs: Ma1Length(numericsimple); //短均线bar数量
inputs: Ma2Length(numericsimple); //长均线bar数量
inputs: Ma1(numericref); //短均线值
inputs: Ma2(numericref); //长均线值

	
Ma1 = AverageFC(Close, Ma1Length) ;
Ma2 = AverageFC(Close, Ma2Length) ;