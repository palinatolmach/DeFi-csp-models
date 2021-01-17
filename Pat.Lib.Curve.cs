using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PAT.Lib
{
    public class Curve
    {
		public static long N_COINS = 2;
		public static long A = 450;
		public static double _fee = 0.0004; // 0.04;
		public static double utilRate = 0;

		public static long D;

		public static int[] exchangeRateStored(int totalCash, int totalBorrows, int totalReserves, int totalSupply)
		{
			long cash = (long)(totalCash);
			long borrows = (long)(totalBorrows);
			long reserves = (long)(totalReserves);
			long supply = (long)(totalSupply);
			double USDC_rate; double DAI_rate;
			int[] returnRates = new int[2];

			USDC_rate = 1.0d * (cash + borrows - reserves) / supply;
			// a rate for the cDAI mock
			DAI_rate = 1.0d * 0.02082491058;

			double USDC_res_rate = 100000.0d * USDC_rate;
			double DAI_res_rate =  100000.0d * DAI_rate;
	        int USDC_returnRate = (int)System.Math.Round(USDC_res_rate);
	        int DAI_returnRate = (int)System.Math.Round(DAI_res_rate);

	        int[] res = new int[] {USDC_returnRate, DAI_returnRate};
	        return res;
		}

		public static int calcMintCUSDC(int mintAmount, int[] rates)
		{
			double[] long_rates = rates.Select(z => z / 100000.0d).ToArray();
			double mintTokens = 1.0d * mintAmount / long_rates[0];
	        int res = (int)System.Math.Round(mintTokens);
	        return res;
		}

		public static int calcRedeemCUSDC(int redeemAmount, int[] rates)
		{
			double[] long_rates = rates.Select(z => z / 100000.0d).ToArray();
			double redeemTokens = 1.0d * redeemAmount * long_rates[0];
	        int res = (int)System.Math.Round(redeemTokens);
	        return res;
		}

		public static int getUtilRate(int totalCash, int totalBorrows, int totalReserves)
		{
			long cash = (long)(totalCash);
			long borrows = (long)(totalBorrows);
			long reserves = (long)(totalReserves);

			utilRate = 1.0d * borrows / (cash + borrows - reserves);
	        int returnRate = (int)System.Math.Round(100.0d * utilRate);
	        return returnRate;
		}

		public static double getBorrowRate()
		{
			double kink = 0.8;
			double multiplierPerBlock = 0.002378234;
			double jumpMultiplerPerBlock = 0.51845509;
			double baseRatePerBlock = 0;
			double borrowRate = 0;

			if (utilRate <= kink) {
				borrowRate = utilRate * multiplierPerBlock + baseRatePerBlock;
			} else {
				double normalRate = kink * multiplierPerBlock + baseRatePerBlock;
				double excessUtil = utilRate - kink;
				borrowRate = normalRate + (excessUtil  * jumpMultiplerPerBlock);
			}
	        return borrowRate;
		}


		public static int[] accrueInterest(int curBlock, int accBlock, int borrows, int reserves, int borrowIndex)
		{
			double borrowRate = getBorrowRate();
			int blockDelta = curBlock - accBlock;
			double simpleInterestFactor = borrowRate * blockDelta;
			double interestAccumulated = simpleInterestFactor * borrows;

			int totalBorrowsNew = (int)(System.Math.Round(interestAccumulated)) + borrows;
			int totalReservesNew = (int)(System.Math.Round(interestAccumulated * 0.075)) + reserves;
			int borrowIndexNew = (int)(System.Math.Round(simpleInterestFactor * borrowIndex)) + borrowIndex;
			int accBlockNew = curBlock;
			int resInterest = (int)(System.Math.Round(interestAccumulated));

			int[] res = new int[] {accBlockNew, borrowIndexNew, totalBorrowsNew, totalReservesNew, resInterest};
	        return res;
		}

		public static int calcRemoveAmount(int dy, int[] rates)
		{
			double[] long_rates = rates.Select(z => z / 100000.0d).ToArray();
			double removeTokens = 1.0d * dy / long_rates[0];
	        int res = (int)System.Math.Round(removeTokens);
	        return res;
		}

		public static int cDAI_exchangeRateStored()
		{ // A mock function
			double cDAI_rate = 1.0d * 0.02082491058;
			double res_rate = 100000.0d * cDAI_rate;
	        int returnRate = (int)System.Math.Round(res_rate);
	        return returnRate;
		}

		public static int sum(int[] balances) {
			return balances.Sum();
		}

		public static int borrowBalanceStored(int accPrincipal, int borrowIndex, int accIndex) {
			long res;
            res = (long)(accPrincipal) * (long)(borrowIndex) / (long)(accIndex);
			return (int)(res % Int32.MaxValue);
		}

		public static long getD(long[] xp) {
			long[] long_xp = xp; // xp.Select(z => (long)z).ToArray();
			long S = long_xp[0] + long_xp[1];
			long Dprev = 0;
			long D = (long)(S);
			long Ann = A * 2;
			long D_P;
			for (var _i = 0; _i < 256; _i++) {
				D_P = D;
				D_P = D_P * D / (long_xp[0] * N_COINS + 1);
				D_P = D_P * D / (long_xp[1] * N_COINS + 1);
				Dprev = D;
				D = (Ann * S + D_P * N_COINS) * D / ((Ann - 1) * D + (N_COINS + 1) * D_P);
				if (D > Dprev) {
					if ((D - Dprev) <= 1) {
						break;
					}
				} else {
					if ((Dprev - D) <= 1) {
						break;
					}
				}
			}
			return D;
		}

		public static int getD(int[] xp) {
			long[] long_xp = xp.Select(z => (long)z).ToArray();
			long S = long_xp[0] + long_xp[1];
			long Dprev = 0;
			long D = (long)(S);
			long Ann = A * 2;
			long D_P;
			for (var _i = 0; _i < 256; _i++) {
				D_P = D;
				D_P = D_P * D / (long_xp[0] * N_COINS + 1);
				D_P = D_P * D / (long_xp[1] * N_COINS + 1);
				Dprev = D;
				D = (Ann * S + D_P * N_COINS) * D / ((Ann - 1) * D + (N_COINS + 1) * D_P);
				if (D > Dprev) {
					if ((D - Dprev) <= 1) {
						break;
					}
				} else {
					if ((Dprev - D) <= 1) {
						break;
					}
				}
			}
			return (int)(D % Int32.MaxValue);
		}

		public static long getY(int i, long[] long_xp, long D_arg) {
//			long[] long_xp = xp.Select(z => (long)z).ToArray();
			long S_ = 0;
			long D_y = D_arg;
			long c = D_y;
			long Ann = A * N_COINS;
			long y;
			long _x = 0;

			for (var _i = 0; _i < 2; _i++) {
				if (_i != i) {
					_x = long_xp[_i];
				} else {
					continue;
				}
				S_ += _x;
				c = c * D_y / (_x * N_COINS);
			}
			c = c * D_y / (Ann * N_COINS);
			long b = S_ + (D_y / Ann);
			long y_prev = 0;
			y = D_y;
			for (var _i = 0; _i < 256; _i++) {
			   	y_prev = y;
			   	y = (y * y + c) / (2 * y + b - D_y);
			   	if (y > y_prev) {
			   		if ((y - y_prev) <= 1) {
			   			break;
			   		}
			   	} else {
					if ((y_prev - y) <= 1) {
						break;
					}
				}
			}
			return y;
		}

		public static long swapGetY(int i, int j, long x, long[] xp) {
//			long[] long_xp = xp.Select(z => (long)z).ToArray();
			long S_ = 0;
			long D = getD(xp);
			long c = D;
			long Ann = A * 2;
			long y;
			long _x = 0;

			for (var _i = 0; _i < 2; _i++) {
				if (_i == i) {
					_x = x;
				} else {
					if (_i != j) {
						_x = xp[_i];
					} else {
						continue;
					}
				}
				S_ += _x;
				c = c * D / (_x * N_COINS);
			}
			c = c * D / (Ann * N_COINS);
			long b = S_ + D / Ann;
			long y_prev = 0;
			y = D;
			for (var _i = 0; _i < 256; _i++) {
			   	y_prev = y;
			   	y = (y * y + c) / (2 * y + b - D);
			   	if (y > y_prev) {
			   		if ((y - y_prev) <= 1) {
			   			break;
			   		}
			   	} else {
					if ((y_prev - y) <= 1) {
						break;
					}
				}
			}
			return y;
		}

		public static int[] removeLiquidityImbalance(int[] bals, int _amounts, int[] rates, int total_supply) {
			long[] long_balances = bals.Select(z => (long)z).ToArray();
			double[] long_rates = rates.Select(z => z / 100000.0d).ToArray();
			long amounts = (long)(_amounts);
			long token_supply = (long)(total_supply);
			long[] old_balances = long_balances;
			long[] newBals = oldBals;
			long[] long_xp = new long[2];
			double fee = _fee * N_COINS / (4 * (N_COINS - 1));
			double[] fees = new double[2];

			long_xp[0] = (long)Math.Round(oldBals[0] * long_rates[0]);
			long_xp[1] = (long)Math.Round(oldBals[1] * long_rates[1]);

			long D_0 = getD(long_xp);

			newBals[0] -= amounts;

			long_xp[0] = (long)Math.Round(newBals[0] * long_rates[0]);
			long_xp[1] = (long)Math.Round(newBals[1] * long_rates[1]);
			long D_1 = getD(long_xp);

			for (var _i = 0; _i < 2; _i++) {
				long ideal_balance = D_1 * oldBals[_i] / D_0;
				long difference = 0;
				if (ideal_balance > newBals[_i]) {
					difference = ideal_balance - newBals[_i];
				} else {
					difference = newBals[_i] - ideal_balance;
				}
				fees[_i] = fee * difference;
				long_balances[_i] = (long)(System.Math.Round(newBals[_i] - (fees[_i] * 0.5)));
				newBals[_i] = (long)(System.Math.Round(newBals[_i] - fees[_i]));
			}

			long_xp[0] = (long)Math.Round(newBals[0] * long_rates[0]);
			long_xp[1] = (long)Math.Round(newBals[1] * long_rates[1]);

			long D_2 = getD(long_xp);

			long token_amount = (D_0 - D_2) * token_supply / D_0;
			int int_amount = (int)(token_amount % Int32.MaxValue);
//			int[] int_balances = long_balances.Select(z => (int)z % Int32.MaxValue).ToArray();

			int a,b;
			a = (int) (long_balances[0]);
			b = (int) (long_balances[1]);

			int[] res = new int[3];
			res = new int[] {int_amount,a,b};

			return res;
		}

		public static int[] curveMintAmount(int[] balances, int amounts, int[] rates, int total_supply) {
			long amount = (long)(amounts);
			long token_supply = (long)(total_supply);
			long[] long_balances = balances.Select(z => (long)z).ToArray();
			double[] long_rates = rates.Select(z => z / 100000.0d).ToArray();
			double fee = _fee * N_COINS / (4 * (N_COINS - 1));
			double[] fees = new double[2];
			long[] long_xp = new long[2];

			long[] oldBals = long_balances;
			long_xp[0] = (long)Math.Round(oldBals[0] * long_rates[0]);
			long_xp[1] = (long)Math.Round(oldBals[1] * long_rates[1]);
			long D_0 = getD(long_xp);

			long[] newBals = oldBals;
			newBals[0] += amount;
			long_xp[0] = (long)Math.Round(newBals[0] * long_rates[0]);
			long_xp[1] = (long)Math.Round(newBals[1] * long_rates[1]);
			long D_1 = getD(long_xp);

			long D_2 = D_1;

			for (var _i = 0; _i < 2; _i++) {
				long ideal_balance = D_1 * oldBals[_i] / D_0;
				long difference = 0;
				if (ideal_balance > newBals[_i]) {
					difference = ideal_balance - newBals[_i];
				} else {
					difference = newBals[_i] - ideal_balance;
				}

				fees[_i] = fee * difference;
				long_balances[_i] = (long)(System.Math.Round(newBals[_i] - (fees[_i] * 0.5)));
				newBals[_i] = (long)(System.Math.Round(newBals[_i] - fees[_i]));
			}

			long_xp[0] = (long)Math.Round(newBals[0] * long_rates[0]);
			long_xp[1] = (long)Math.Round(newBals[1] * long_rates[1]);

			D_2 = getD(long_xp);

			long mint_amount = token_supply * (D_2 - D_0) / D_0;

			int int_mint = (int)(mint_amount % Int32.MaxValue);
			int a,b;

//			int[] int_balances = long_balances.Select(z => (int)z % Int32.MaxValue).ToArray();
			a = (int) (long_balances[0]);
			b = (int) (long_balances[1]);

			int[] res = new int[3];
			res = new int[] {int_mint,a,b};

			return res;
		}

		public static int calcWithdrawOneCoin(int i, int _token_amount, int total_supply, int[] balances, int[] rates) {
			long D_0 = 0;
			long D_1 = 0;
			long token_amount = (long)(_token_amount);
			long token_supply = (long)(total_supply);
			long[] long_balances = balances.Select(z => (long)z).ToArray(); // = swapBalances
			double[] long_rates = rates.Select(z => z / 100000.0d).ToArray();
			long[] xp_reduced = new long[2];
			long[] long_xp = new long[2];
			long dy;

			double fee = _fee * N_COINS / (4 * (N_COINS - 1));
			fee += fee; // Fee overcharging due to imprecision

			long_xp[0] = (long)Math.Round(long_balances[0] * long_rates[0]);
			long_xp[1] = (long)Math.Round(long_balances[1] * long_rates[1]);
			long S = long_xp[0] + long_xp[1];

			D_0 = getD(long_xp);
			D_1 = D_0 - (token_amount * D_0 / token_supply);

			xp_reduced = long_xp;

			for (var _i = 0; _i < 2; _i++) {
				long dx_expected = 0;
				long b_ideal = long_xp[_i] * D_1 / D_0;
				long b_expected = long_xp[_i];
				if (_i == i) {
					b_expected -= S * (D_0 - D_1) / D_0;
				}
				if (b_ideal >= b_expected) {
					dx_expected = (b_ideal - b_expected);
				} else {
					dx_expected = (b_expected - b_ideal);
				}
        		xp_reduced[_i] = (long)(System.Math.Round(xp_reduced[_i] - (fee * dx_expected)));
			}

			long _y = getY(i,xp_reduced,D_1);
			dy = xp_reduced[i] - _y;

			return (int)(dy % Int32.MaxValue);
		}

		public static int[] _exchange(int i, int j, int dx, int[] bals, int[] rates) {
			long long_dx = (long)(dx);
			long[] long_xp = new long[2];
			long[] long_balances = bals.Select(z => (long)z).ToArray();
			double[] long_rates = rates.Select(z => z / 100000.0d).ToArray();

			long_xp[0] = (long)Math.Round(long_balances[0] * long_rates[0]);
			long_xp[1] = (long)Math.Round(long_balances[1] * long_rates[1]);

			long x = long_xp[i] + (long)Math.Round(dx * long_rates[i]);
			long y = swapGetY(i,j,x,long_xp);
			long dy = long_xp[j] - y;
			double dy_fee = dy * _fee;
			double dy_admin_fee = dy_fee * 0.5;
			long_balances[i] = (long)Math.Round(x / long_rates[i]);
			long_balances[j] = (long)Math.Round((y + (dy_fee - dy_admin_fee)) / long_rates[j]);

			var _dy = (dy - dy_fee) / long_rates[j];

//			int[] int_balances = swapBalances.Select(z => (int)z % Int32.MaxValue).ToArray();
			int int_dy = (int)(_dy % Int32.MaxValue);

			int a,b;
			a = (int) (long_balances[0]);
			b = (int) (long_balances[1]);

			int[] res = new int[3];
			res = new int[] {int_dy,a,b};

			return res;
		}

		public static void LogVariable(string name, object variable)
		{
		    string path = @"C:\Users\User\Desktop\log.txt";

		    using (var sw = System.IO.File.AppendText(path)) {
		        sw.Write(System.DateTime.Now.ToLocalTime().ToShortTimeString());
		        sw.Write("\t");
		        sw.Write("'" + name + "' (" + variable.GetType() + ") :");
		        sw.Write("\t");
		        if(typeof(System.Collections.IEnumerable).IsInstanceOfType(variable) && !typeof(string).IsInstanceOfType(variable)) {
		            var ienum = ((System.Collections.IEnumerable)variable).Cast<Object>().Select(x => x.ToString()).ToArray();
		            sw.Write("{");
		            sw.Write(String.Join(",", ienum));
		            sw.Write("}");
		        }
		        else {
		            sw.Write(variable);
		        }
		        sw.WriteLine();
		    }
		}

    }
}