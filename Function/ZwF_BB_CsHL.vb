//ZwF_BB_CsHL，获取K线与boll上轨交叉点的最高点（BB_Cu_H），与下轨交叉点的最低点（BB_Cd_L）
////---布林指标值------------////
inputs:	BBPrice(numericseries); //布林指标-计算基础数据参数
inputs:	BBLength(numericsimple); //布林指标-长度参数
inputs:	NumD(numericsimple); //布林指标-标准差倍数参数
//---布林指标返回值------------
inputs:	MAbb(numericref); //布林指标-参考均线
inputs: BollUp(numericref); //布林指-标上轨
inputs: BollDn(numericref); //布林指-标下轨

value1 = ZwF_BB(BBPrice,BBLength,NumD,MAbb,BollUp,BollDn);

////---布林交叉点HL指标------------////
inputs: BB_Cu_H(numericref); //突破布林上轨K线最高点
inputs: BB_Cu_Ma(numericref); //突破布林上轨布林均线值
inputs: BB_Cd_L(numericref); //突破布林下轨K线最低点
inputs: BB_Cd_Ma(numericref); //突破布林下轨布林均线值
inputs: DKBoll1(numericref); //多空标志-布林突破（突破上轨1，突破下轨-1）
inputs: DKBoll2(numericref); //多空标志-布林突破（突破上轨点1，突破下轨点-1，其余0）

if Close crosses above BollUp then//
	begin
	BB_Cu_H = High;
	BB_Cu_Ma = MAbb;
	DKBoll1 = 1;
	DKBoll2 = 1;
	end
else if Close crosses below BollDn then//
	begin
	BB_Cd_L = Low;
	BB_Cd_Ma = MAbb;
	DKBoll1 = -1;
	DKBoll2 = -1;
	end
else
	begin
	DKBoll2 = 0;
	end;