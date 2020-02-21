//ZwF_RSI_D1_s_D2，价差(D1-D2)及价差(D1-D2)RSI

inputs: RSI_Len(numericsimple); ; //RSI长度参数
inputs: D1_s_D2(NumericRef); //价差(D1-D2)
inputs: RSI_D1_s_D2(NumericRef); //价差(D1-D2)RSI

D1_s_D2 = Close of data1 - Close of data2;
RSI_D1_s_D2 = RSI(D1_s_D2,RSI_Len);