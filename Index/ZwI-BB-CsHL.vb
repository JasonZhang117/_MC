//Zw-I-BB-CsHL，布林通道上下轨交叉点的高低点
////---布林指标值------------////
inputs:	BBLength(50); //布林指标-长度参数
//---布林指标返回值------------
variables: MAbb(Close); //布林指标-参考均线
variables: BollUp(High); //布林指-标上轨
variables: BollDn(Low); //布林指-标下轨

////---布林交叉点HL指标------------////
variables: BB_Cu_H(High); //突破布林上轨K线最高点
variables: BB_Cu_Ma(Low); //突破布林上轨布林均线值
variables: BB_Cd_L(Low); //突破布林下轨K线最低点
variables: BB_Cd_Ma(High); //突破布林下轨布林均线值
variables: DKBoll1(0); //多空标志-布林突破（突破上轨1，突破下轨-1）
variables: DKBoll2(0); //多空标志-布林突破（突破上轨点1，突破下轨点-1，其余0）

value1 = ZwF_BB_CsHL(Close,BBLength,2,MAbb,BollUp,BollDn,BB_Cu_H,BB_Cu_Ma,BB_Cd_L,BB_Cd_Ma,DKBoll1,DKBoll2);

plot1(MAbb,"MAbb");
plot2(BollUp,"BollUp");
plot3(BollDn,"BollDn");
plot4(BB_Cu_H,"BB_Cu_H");
plot5(BB_Cu_Ma,"BB_Cu_Ma");
plot6(BB_Cd_L,"BB_Cd_L");
plot7(BB_Cd_Ma,"BB_Cd_Ma");