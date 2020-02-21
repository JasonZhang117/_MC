//ZwF_BB，布林通道
////---布林指标值------------////
inputs:	BBPrice(numericseries); //布林指标-计算基础数据参数
inputs:	BBLength(numericsimple); //布林指标-长度参数
inputs:	NumD(numericsimple); //布林指标-标准差倍数参数
//---布林指标返回值------------
inputs:	MAbb(numericref); //布林指标-参考均线
inputs: BollUp(numericref); //布林指-标上轨
inputs: BollDn(numericref); //布林指-标下轨	
	
variables:
	Stdv(0);

Stdv = StandardDev(BBPrice, BBLength, 1 ) ;

MAbb = AverageFC(BBPrice, BBLength) ;
BollUp = MAbb + NumD * Stdv;
BollDn = MAbb - NumD * Stdv;

