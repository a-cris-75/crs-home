using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using PoseidonData.DataEntities;
using CRS.Library;
using CRS.CommonControlsLib;
using Excel = Microsoft.Office.Interop.Excel;
using Infragistics.Win.UltraWinChart;


namespace PoseidonApp.UserControls
{
    public partial class PSDChartAnalysis : UserControl
    {
        public PSDChartAnalysis()
        {
            InitializeComponent();
        }
        public delegate void EventSelectPeriod();
        private EventSelectPeriod eventSelectPeriod;

        public void SetEventSelectPeriod(EventSelectPeriod evn)
        {
            this.eventSelectPeriod = evn;
        }

        private List<PSD_RES_TOT> DATA_RESULTS = new List<PSD_RES_TOT>();
        public void SetDataResults(List<PSD_RES_TOT> data_results)
        {
            try {
                this.DATA_RESULTS = data_results;
                DataTable dt = Utils.ToDataTable<PSD_RES_TOT>(data_results);
                cbColValX.Items.Clear();
                cbColValY.Items.Clear();
                foreach (DataColumn c in dt.Columns)
                {
                    cbColValX.Items.Add(c.ColumnName);
                    cbColValY.Items.Add(c.ColumnName);
                    cbColValZ.Items.Add(c.ColumnName);
                }
            }
            catch { }
        }

        public void Init(List<PSD_RES_TOT> res)
        {
            this.SetDataResults(res);
        }

        public void AddSeries(List<PSD_RES_TOT> res, string nameSeries, string colNameX, string colNameY, string colNameZ, SeriesChartType t, Chart cht)
        {
            Series ser = new Series(nameSeries);
            ser.ChartType = t;//SeriesChartType.Line;
            ser.ChartArea = "ChartArea1";
            chtPSD_L.ChartAreas[0].Area3DStyle.Enable3D = false;
            if (!string.IsNullOrEmpty(colNameZ))
                cht.ChartAreas[0].Area3DStyle.Enable3D = true; 
            
            DataTable dt = Utils.ToDataTable<PSD_RES_TOT>(res);

            try
            {
                ChartValueType tx = new ChartValueType();
                ChartValueType ty = new ChartValueType();
                //ChartValueType tz = new ChartValueType();
                Type tc = dt.Columns[colNameX].GetType();
                if (tc == typeof(string))
                    tx = ChartValueType.String;
                if (tc == typeof(int))
                    tx = ChartValueType.Int32;
                if (tc == typeof(DateTime))
                    tx = ChartValueType.DateTime;
                if (tc == typeof(Single))
                    tx = ChartValueType.Single;
                if (tc == typeof(float) || tc == typeof(Double))
                    tx = ChartValueType.Double;

                tc = dt.Columns[colNameY].GetType();
                if (tc == typeof(string))
                    ty = ChartValueType.String;
                if (tc == typeof(int))
                    ty = ChartValueType.Int32;
                if (tc == typeof(DateTime))
                    ty = ChartValueType.DateTime;
                if (tc == typeof(Single))
                    ty = ChartValueType.Single;
                if (tc == typeof(float) || tc == typeof(Double))
                    ty = ChartValueType.Double;
                /*
                if (!string.IsNullOrEmpty(colNameZ)) { 
                    tc = dt.Columns[colNameZ].GetType();
                    if (tc == typeof(string))
                        tz = ChartValueType.String;
                    if (tc == typeof(int))
                        tz = ChartValueType.Int32;
                    if (tc == typeof(DateTime))
                        tz = ChartValueType.DateTime;
                    if (tc == typeof(Single))
                        tz = ChartValueType.Single;
                    if (tc == typeof(float) || tc == typeof(Double))
                        tz = ChartValueType.Double;
                }*/

                ser.XValueType = tx;
                ser.YValueType = ty;

                List<Point3D> pa = new List<Point3D>();
                foreach (DataRow r in dt.Rows)
                {
                    ser.Points.AddXY(r[colNameX], r[colNameY]);
                    if (!string.IsNullOrEmpty(colNameZ))
                    {
                        Point3D p = new Point3D(Convert.ToSingle(r[colNameX]), Convert.ToSingle(r[colNameY]), Convert.ToSingle(r[colNameZ]));
                        pa.Add(p);
                    }
                }
                cht.Series.Add(ser);
                if (!string.IsNullOrEmpty(colNameZ))
                {
                    cht.ChartAreas[0].TransformPoints(pa.ToArray());
                }
            }
            catch(Exception ex) {
                ABSMessageBox.Show("ERRORE: " + ex.Message);
            }
        }

        public void SumSeriesLine(Series ser1, Series ser2)
        {
            Series ser = new Series("SUM " + ser1.Name + "-" + ser2.Name);
            ser.ChartType = SeriesChartType.Line;
            ser.ChartArea = "ChartArea1";
            chtPSD_L.ChartAreas[0].Area3DStyle.Enable3D = false;

            try
            {
                int idx = 0;
                foreach (DataPoint p in ser1.Points)
                {
                    ser.Points.AddXY(p.XValue, p.YValues.First() + ser2.Points[idx].YValues.First());
                }

                ser.XValueType = ser1.XValueType;
                ser.YValueType = ser1.YValueType;

                chtPSD_L.Series.Add(ser);
                
            }
            catch (Exception ex)
            {
                ABSMessageBox.Show("ERRORE: " + ex.Message);
            }
        }

        public void MoveIntoPriod(Series ser, int idxmin, int idxmax)
        {

            //ser.Points.DataBindXY(xValues, yValues);
            //foreach (DataPoint dp in ser.Points)
            chtPSD_L.ChartAreas[0].AxisX.Minimum = idxmin;
            chtPSD_L.ChartAreas[0].AxisX.Maximum = idxmax;
        }

        private void Zoom(int delta, bool zoomX, bool zoomY)
        {
            chtPSD_L.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
           
            double xMin = chtPSD_L.ChartAreas[0].AxisX.ScaleView.ViewMinimum - (float)delta / 2;
            double xMax = chtPSD_L.ChartAreas[0].AxisX.ScaleView.ViewMaximum + (float)delta / 2; 
            double yMin = chtPSD_L.ChartAreas[0].AxisY.ScaleView.ViewMinimum - (float)delta / 2; 
            double yMax = chtPSD_L.ChartAreas[0].AxisY.ScaleView.ViewMaximum + (float)delta / 2; 

            /*double posXStart = chtPSD.ChartAreas[0].AxisX.PixelPositionToValue(e.Location.X) - (xMax - xMin) / 4;
            double posXFinish = chtPSD.ChartAreas[0].AxisX.PixelPositionToValue(e.Location.X) + (xMax - xMin) / 4;
            double posYStart = chtPSD.ChartAreas[0].AxisY.PixelPositionToValue(e.Location.Y) - (yMax - yMin) / 4;
            double posYFinish = chtPSD.ChartAreas[0].AxisY.PixelPositionToValue(e.Location.Y) + (yMax - yMin) / 4;*/
            if (zoomX) chtPSD_L.ChartAreas[0].AxisX.ScaleView.Zoom(xMin, xMax);
            if (zoomY) chtPSD_L.ChartAreas[0].AxisY.ScaleView.Zoom(yMin, yMax);
            

        }

        private void MoveAxis(int delta, bool moveX, bool moveY)
        {
            chtPSD_L.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            
            double xMin = chtPSD_L.ChartAreas[0].AxisX.ScaleView.ViewMinimum + (float)delta;
            double xMax = chtPSD_L.ChartAreas[0].AxisX.ScaleView.ViewMaximum + (float)delta;
            double yMin = chtPSD_L.ChartAreas[0].AxisY.ScaleView.ViewMinimum + (float)delta; 
            double yMax = chtPSD_L.ChartAreas[0].AxisY.ScaleView.ViewMaximum + (float)delta;

            if (moveX) chtPSD_L.ChartAreas[0].AxisX.ScaleView.Zoom(xMin, xMax);
            if (moveY) chtPSD_L.ChartAreas[0].AxisY.ScaleView.Zoom(yMin, yMax);


        }

        #region SPECIAL LINES
        // costruisco la sinusoide usando un punto che si muove sul diametro verticale di un cerchio
        // che rotola. Ad ogni movimento il punto sul diametro verticale si muove essendo la proiezione
        // del raggio del cerchio che si sposta con la rotazione
        // il valore di y per ogni x è quindoi nient'altro che il valore del seno calcolato per ogni
        // angolo di rotazione del cerchio che avviene ad ogni estrazione
        private Series DisegnaLineaSinusoide(Series ParentLine, float altezzaCentro, float radiantiRotazionePerEstr, float maxPercDiffLen, out double len, out double lenP, int startX, string title)
        {
            Series linea = new Series();
            linea = CalcolaValoriDiSinusoidePerGrafico(ParentLine, altezzaCentro, radiantiRotazionePerEstr,  out len, out lenP, maxPercDiffLen,startX);
            linea.Legend = "Sinusoide" + " - L: " + len.ToString("0.000")+ " - LP: " + lenP.ToString("0.000");
            
            return linea;
        }

        // costruisco la sinusoide usando un punto che si muove sul diametro verticale di un cerchio
        // che rotola. Ad ogni movimento il punto sul diametro verticale si muove essendo la proiezione
        // del raggio del cerchio che si sposta con la rotazione
        // il valore di y per ogni x è quindi nient'altro che il valore del seno calcolato per ogni
        // angolo di rotazione del cerchio che avviene ad ogni estrazione
        // PARAMETRI: 
        //      - altezzaCentro = ALTEZZA Y DEL CENTRO DEL CERCHIO (è IL RAGGIO)
        //      - radiantiRotazionePerEstr = 
        private Series CalcolaValoriDiSinusoidePerGrafico(Series parentLine, float altezzaCentro, float radiantiRotazionePerEstr,
                            out double len, out double lenParent,                           
                            float maxPercDiffLen,
                            int startX)
       {
            Series linea = new Series();
            double yval = 0;
            double totyval = 0;
            double yvalprec = 0;
            float miny = 0;
            float mediay = 0;
            float g = 0;
            float percDiffLen = 0;
            float k = 0; 
            int numX;
            
            int x0 = startX;

            len = 0;
            lenParent = 0;

            percDiffLen = maxPercDiffLen;
            if (maxPercDiffLen<=0) percDiffLen = (float)0.01;

            numX =  parentLine.Points.Count; 
            miny = (float)parentLine.Points.FindMinByValue("Y").YValues.Min();
            
            foreach(DataPoint p in parentLine.Points)
            {
                yval = Convert.ToSingle( p.YValues.First());
                totyval = totyval + yval;
                lenParent = lenParent + Math.Sqrt(1+(yval - yvalprec) * (yval - yvalprec));
                yvalprec = yval;
            }
         
            // devo copiare tutti i valori dell'asse X che comrendono anche le date
	        /*   for I := x0 to parentLine.YValues.Count-1 do begin
            linea.AddXY(parentLine.XValue[i],0,'', clRed);
                linea.XLabel[i - startX]:= parentLine.XLabel[i];
            end;*/

            // altezza del centro del cerchio che uso per disegnare la sinusoide
            mediay = altezzaCentro;
            // se passo un parametro<=0 significa che desidero usare una media di tutte i valori della linea
            // del grafico che genera la sinuisoide
            if (altezzaCentro<=0) 
                mediay = (float) totyval / (numX - x0);

            len = 0;
            g = radiantiRotazionePerEstr;

            if (g <= 0)
            {
                // se il parametro è 0 calcolo la sinusoide finchè la sua lunghezza si avvicina a quella
                // della linea generatrice (lenP)
                len = 0;
                // indicano se l'ultimo calcolo della lunghezza e quello corrente sono inferiori alla lunghezza della linea di origine
                // servono per stabilire il parametro g che viene ridotto in scala se non riesco a rispettare la percentuale di differenza di linee
                // voluta
                //bPreviousCalcIsInf = true;
                //bCurrentCalcIsInf = true;
                k = (float)0.1;
                // considero una deviazione del 5% dalla linea originale
                //while (len>lenP)and(x0<numX) do begin
                while ((Math.Abs(len - lenParent) / lenParent) * 100 > percDiffLen)
                {
                    yvalprec = 0;
                    len = 0;
                    x0 = startX;
                    while (x0 < numX)
                    {
                        //yVal:= mediay + (mediay-miny) * (sin(g*(x0-startX)));
                        // faccio partire la sinusoide dal punto più basso (-PI_GRECO)
                        yval = mediay + (mediay - miny) * (Math.Sin((-Math.PI / 2) + g * (x0 - startX)));
                        linea.Points.AddXY(x0, yval);

                        x0++;

                        len = len + Math.Sqrt(1 + (yval - yvalprec) * (yval - yvalprec));
                        yvalprec = yval;
                    }

                    k = k / 10;
                    if (len > lenParent)
                    {                      
                        //bPreviousCalcIsInf = bCurrentCalcIsInf;
                        //bCurrentCalcIsInf = false;
                        g = g - k;
                    }
                    else
                    {
                        //bPreviousCalcIsInf = bCurrentCalcIsInf;
                        //bCurrentCalcIsInf = true;
                        g = g + k;
                    }
                }
            }
            else
            {
                yvalprec = 0;
                while (x0 < numX - 1)
                {
                    yval = mediay + (mediay - miny) * (Math.Sin(g * (x0 - startX)));
                    linea.Points.AddXY(x0, yval);

                    x0++;

                    len = len + Math.Sqrt(1 + (yval - yvalprec) * (yval - yvalprec)); //1 è la distanza fra un'estr e l'altra
                    yvalprec = yval;
                }
            }
       
            return linea;
        }
        #endregion

        #region EVENTS
        private void btnAddSeries_Click(object sender, EventArgs e)
        {
            try
            {
               string z = cbColValZ.Text;
                if (string.IsNullOrEmpty(z))
                    z = "-" + z;
                string name = cbColValX.Text + "-" + cbColValY.Text + z + "  " + this.DATA_RESULTS.First().DATA_EVENTO_DA.ToShortDateString() + "-" + this.DATA_RESULTS.Last().DATA_EVENTO_A.ToShortDateString();
                AddSeries(this.DATA_RESULTS, name, cbColValX.Text, cbColValY.Text, cbColValZ.Text, SeriesChartType.Line, chtPSD_L); 
            }
            catch(Exception ex) {
                ABSMessageBox.Show("ERRORE: " + ex.Message);
            }
       }

        private void btnClearLastSeries_Click(object sender, EventArgs e)
        {
            chtPSD_L.Series.RemoveAt(chtPSD_L.Series.Count - 1);
        }

        private void btnChartZoomPlus_Click(object sender, EventArgs e)
        {
            try
            {
                Zoom(-Convert.ToInt32(txtDeltaZoom.Text), chkZoomX.Checked, chkZoomY.Checked);
            }
            catch { }
        }

        private void btnChartZoomMinus_Click(object sender, EventArgs e)
        {
            try
            {
                Zoom(Convert.ToInt32(txtDeltaZoom.Text), chkZoomX.Checked, chkZoomY.Checked);
            }
            catch { }
        }

        private void btnChartLeft_Click(object sender, EventArgs e)
        {
            MoveAxis(-Convert.ToInt32(txtDeltaZoom.Text), true, false);
        }

        private void btnResetZoom_Click(object sender, EventArgs e)
        {
            chtPSD_L.ChartAreas[0].AxisX.ScaleView.ZoomReset();
            chtPSD_L.ChartAreas[0].AxisY.ScaleView.ZoomReset();
           
        }

        private void btnChartRight_Click(object sender, EventArgs e)
        {
            MoveAxis(Convert.ToInt32(txtDeltaZoom.Text), true, false);
        }

        private void chk3D_Click(object sender, EventArgs e)
        {
            chtPSD_L.ChartAreas[0].Area3DStyle.Enable3D = chk3D.Checked;
        }
        #endregion

        #region DA DELPHI
        /*
        // costruisce la linea a partire da una generatrice
        function DisegnaLineaFibonacci(ParentChart: TChart; //tipoLineaTChart: integer;
        ParentLine: TChartSeries;
                            StartX: integer = -1; title: string = ''): TpointSeries;//TArrowSeries;
        var linea: TPointSeries;//TArrowSeries;
          x0,dx,yval: single;
        begin
          //linea:=  TArrowSeries.Create(Application);
          linea:=  TPointSeries.Create(Application);
          linea.ParentChart:= ParentChart;

          x0:= ParentLine.MinXValue;
          if StartX<>-1 then x0:= StartX;
  
          //dx:= 1;
          dx:= ParentLine.XValue[1]-ParentLine.XValue[0];
          yval:= ParentLine.XValue[1]-ParentLine.XValue[0]/1.618033;
          while x0<ParentLine.MaxXValue do begin
            x0:= x0 + dx;
            yVal:= dx/1.618033;
            dx:= dx*1.618033;

            //linea.AddArrow(x0,0,x0,yval,floatToStr(yVal));
            linea.AddXY(x0,yval,floatToStr(yVal),clRed);
          end;

          linea.ShowInLegend:= True;
          linea.Visible:= True;
          linea.Title:= 'Fibonacci';
          if title<>'' then linea.Title:= title;

                ParentChart.SeriesList[ParentChart.SeriesCount - 1]:= linea;
          Result:= Linea;
        end;

        // costruisco la sinusoide usando un punto che si muove sul diametro verticale di un cerchio
        // che rotola. Ad ogni movimento il punto sul diametro verticale si muove essendo la proiezione
        // del raggio del cerchio che si sposta con la rotazione
        // il valore di y per ogni x è quindoi nient'altro che il valore del seno calcolato per ogni
        // angolo di rotazione del cerchio che avviene ad ogni estrazione
        function DisegnaLineaSinusoide(ParentChart: TChart; //tipoLineaTChart: integer;
                ParentLine: TChartSeries;
                                    altezzaCentro:single; radiantiRotazionePerEstr: single; maxPercDiffLen: single;
                                    var len, lenP: single;
                                    StartX: integer = -1; title: string = ''): TLineSeries;
        var linea: TLineSeries;//TArrowSeries;
          x0,i: integer;
          dx,yval,totyval,yvalprec,maxy,miny,mediay,g: single;
          numX: integer;
        begin
          linea:=  TLineSeries.Create(Application);
          linea.ParentChart:= ParentChart;

          calcolaValoriDiSinusoidePerGrafico(ParentLine, altezzaCentro, radiantiRotazionePerEstr, len, lenP, TChartSeries(linea),maxPercDiffLen,StartX);

          linea.ShowInLegend:= True;
          linea.Visible:= True;
          linea.Title:= 'Sinusoide'+ ' - L: ' + format('%2f', [len])+ ' - LP: ' + format('%2f', [lenP]);
          if title<>'' then linea.Title:= title+ ' - L: ' + format('%2f', [len])+ ' - LP: ' + format('%2f', [lenP]);

                ParentChart.SeriesList[ParentChart.SeriesCount - 1]:= linea;
          Result:= Linea;
        end;

        // bisogna interrogarsi su come intrecciare la sinusoide con la generatrice:
        // in altre parole, dove far iniziare la sinusoide rispetto alla generatrice?
        // 	.un buon parametro di form è la lunghezza della generatrice: ok
        //	.potrei far cominciare la sinusoide in un punto di origine della generatrice: ok
        //	.dove far terminare la sinusoide? ora termina in maniera armonica con questo metoso
        //	 NB: in questo modo però ad ogni nuova estrazione considerata il punto di termine
        //				cambia dato che l'armonica cambia
        //			.nella differenza fra le sinusoidi armoniche costruite su due punti terminali diversi
        //				potrebbe esserci qualcosa di interressante..
        procedure DisegnaLineeSinusoideDiscrete(ParentChart: TChart; //tipoLineaTChart: integer;
                ParentLine: TChartSeries;
                                    altezzaCentro:single; radiantiRotazionePerEstr: single; maxPercDiffLen: single;
                                    var len, lenP: single;
                                    StartX: integer = -1; title: string = '');
        var linea: TLineSeries;//TArrowSeries;
          x0,i,j: integer;
          dx,yval,totyval,yvalprec,maxy,miny,mediay,g,y0: single;
          numX: integer;
          a: array of integer;
        begin
          // 1. devo trovare i divisori di numx per determinare le lunghezza d'onda per ottenere
          //	 	la sinusoide "armonica" (cioè che inizia e termina allo stesso valore di y)
          // 2.	usando la formula per determinare y devo trovare il corretto valore di g
          //		al variare di x (dove x è la lunghezza d'onda)
          // 3. per ogni lunghezza d'onda ammessa ricalcola la funzione d'onda scegliendo quella
          // 		che più si avvicina alla lunghezza della linea generatrice

          // a contiene i divisore di numx
          x0:= StartX;
          numX:=  ParentLine.YValues.Count div 2; // più o meno ci sono n valori e n date
                miny:= ParentLine.MinYValue;
	        miny:= ParentLine.MinYValue;
          yval:=0;   totyval:=0; lenP:=0;
          yvalprec:=0;
          y0:= ParentLine.YValue[x0];
          for I := x0 to numX - 1 do begin
              yval:= ParentLine.YValue[i];
            totyval:= totyval + yval;
            // lunghezza linea generatrice: tante ippotenuse di triangoli rett con un cateto = 1 (estr)
            lenP:= lenP + sqrt(1 + (yval-yvalprec)*(yval-yvalPrec));
            yvalPrec:= yval;
          end;
          // se passo un parametro<=0 significa che desidero usare una media di tutte i valori della linea
          // del grafico che genera la sinuisoide
          mediay:= altezzaCentro;
          if altezzaCentro<=0 then
              mediay:= totyval / (numX - x0);

          setLength(a, numx div 2);
          for i := 0 to high(a)  do
              a[i]:=-1;
          j:=0;
          i:=2;
          while i<=(numx div 2) do begin
            if (numx mod i) = 0 then begin

                a[j]:= i;
              inc(j);
                end;
            inc(i);
                end;

          for i := 0 to high(a) do begin
  	        if a[i]>0 then begin
                // y = sin(x)   					=> sinusoide con origine nell'origine degli assi cartesiani (0,0)
                // y = k + sin(x)     		=> origine in k
                // y = k + sin(x + h) 		=> origini in y0=k e x0=h
                // y = k + sin(xx*g + h) 	=> origini in y0=k e x0=h, xx*g = x
                // arcsin(y-k) = xx*g + h	=> funzione inversa

                // dalla DEFINIZIONE matematica
                // esempio:
                // y= a*sin(b*x+c) =>  	-c/b 	= spostamento sull'asse x: (b*x + c = 0)
                //											a 		= ampiezza   (altezza cresta)
                //                      2*PGreco / b = periodo
                // nel mio caso:
                // 	.periodo = numero di estrazioni della lunghezza d'onda (a[i])
                //	.coefficiente g = 2*PGreco/periodo
                g:= (2*PI_GRECO) / a[i];


    	        // disegna tutte le linee armoniche con la linea generatrice
    	        // per linea armonica si intende una linea con periodi discreti
  		        linea:=  TLineSeries.Create(Application);
  		        linea.ParentChart:= ParentChart;


                calcolaValoriDiSinusoideDiscreti(ParentLine, altezzaCentro, g, len, lenP, TChartSeries(linea),maxPercDiffLen,StartX);
  		        //calcolaValoriDiSinusoidePerGrafico(ParentLine,altezzaCentro,radiantiRotazionePerEstr,len,lenP,TChartSeries(linea),maxPercDiffLen,StartX);

  		        linea.ShowInLegend:= True;
  		        linea.Visible:= True;
  		        linea.Title:= 'Sinusoide'+ ' - L: ' + format('%2f', [len])+ ' - LP: ' + format('%2f', [lenP]);
  		        if title<>'' then linea.Title:= title+ ' - L: ' + format('%2f', [len])+ ' - LP: ' + format('%2f', [lenP]);

                ParentChart.SeriesList[ParentChart.SeriesCount - 1]:= linea;
            end;
          end;
        end;

        // dalla sinusoide e dalla linea generatrice costruisco una linea in cui per ogni x (estrazione)
        // la y corrisponde alla differenza di coordinate fra Y sinusoide e Y generatrice sinusoide
        function DisegnaLineaDiffSinusoideGeneratrice(ParentChart: TChart;
                ParentLine: TChartSeries;
                                    SinusLine : TLineSeries;
                                    bAbsValues: boolean; // se true considera solo il valore assoluto della differenza
                                    title: string = ''): TLineSeries;
        var linea: TLineSeries;//TArrowSeries;
          x0,i: integer;
          dx,yval,totyval,yvalprec,maxy,miny,mediay,g: single;
          numX: integer;  s: string;
        begin
          linea:=  TLineSeries.Create(Application);
          linea.ParentChart:= ParentChart;

          calcolaValoriDiDiffSinusGenPerGrafico(ParentLine, SinusLine, bAbsValues, TChartSeries(linea));

          linea.ShowInLegend:= True;
          linea.Visible:= True;
          s:= '';
          if bAbsValues then s:= ' - ABS values ';
  
          linea.Title:= 'Diff Sinusoide/Generatrice'+ s;
          if title<>'' then linea.Title:= title + s;

                ParentChart.SeriesList[ParentChart.SeriesCount - 1]:= linea;
          Result:= Linea;
        end;


        // creo una linea del grafico come risultante delle linee selezionate presenti
        // il valore di y rappresenta la somma di tutti gli y per ogni x
        function DisegnaLineaSommaLinee(ParentChart: TChart;
                title: string = ''): TLineSeries;
        var linea: TLineSeries;//TArrowSeries;
          i,j: integer;
          yval: single;
          numX: integer;
          ParentLine: TChartSeries;
          s: string;
        begin
          linea:=  TLineSeries.Create(Application);
          linea.ParentChart:= ParentChart;

	        ParentLine:= ParentChart.Series[0];
          numX:=  ParentLine.YValues.Count div 2; // più o meno ci sono n valori e n date

          // devo copiare tutti i valori dell'asse X che comrendono anche le date
	        for I := 0 to ParentLine.YValues.Count-1 do begin
            linea.AddXY(ParentLine.XValue[i],0,'', clRed);
                linea.XLabel[i]:= ParentLine.XLabel[i];
          end;

          for I := 0 to ParentLine.YValues.Count-1 do begin
              yval:=0;
  	        for j:=0 to(parentchart.SeriesCount -1) do begin
  		        if ParentChart.Series[j].Active then begin
                  yval:= yval + ParentChart.Series[j].YValue[i];
    	        end;
            end;
            linea.YValue[i]:= yval;

          end;

          linea.ShowInLegend:= True;
          linea.Visible:= True;
          // - 2 dato che non devo considerare l'ultima linea appena creata
          for i:=0 to(parentchart.SeriesCount -2) do begin
            if ParentChart.Series[i].Active then begin
                s:= s + inttostr(i) + '-';
            end;
          end;

          s:= copy(s,0, length(s)-1);

          linea.Title:= 'Somma Linee ' + s;
          if title<>'' then linea.Title:= title + linea.Title;

                ParentChart.SeriesList[ParentChart.SeriesCount - 1]:= linea;
          Result:= Linea;
        end;


        // costruisco la la linea normalizzata partendo dalla linea originale
        // la normalizzazione consite nel molitplicare il vlore dell'ordinata per 10 e dividerlo per
        // il numero di giocati in un'estrazione (che è costante quasi sempre, ma non distinguo
        // i casi in cui per casi fortuiti(gemelli o giocati in colpi divcersi uguali) non lo è)
        function DisegnaLineaNormalizzata(ParentChart: TChart;
                ParentLine: TChartSeries;
                                    numGiocatiPerEstr: integer;
                                    title: string = ''): TLineSeries;
        var linea: TLineSeries;
          i: integer;
          yval,yvalprec,lenP: single;
          numX: integer;
        begin
          linea:=  TLineSeries.Create(Application);
          linea.ParentChart:= ParentChart;

          numX:=  ParentLine.YValues.Count div 2; // più o meno ci sono n valori e n date
                yval:=0;  lenP:=0;
          yvalprec:=0;
          for I := 0 to numX - 1 do begin
              yval:= ParentLine.YValue[i] * 10 / numGiocatiPerEstr;
            // lunghezza linea generatrice: tante ippotenuse di triangoli rett con un cateto = 1 (estr)
            lenP:= lenP + sqrt(1 + (yval-yvalprec)*(yval-yvalPrec));
            yvalPrec:= yval;
            // init della linea
            linea.AddXY(ParentLine.XValue[i],yval,'',clRed);
          end;

          linea.ShowInLegend:= True;
          linea.Visible:= True;
          linea.Title:= 'Normalizzata'+ ' - L: ' + format('%2f', [lenP]);
          if title<>'' then linea.Title:= title+ ' - L: ' + format('%2f', [lenP]);

                ParentChart.SeriesList[ParentChart.SeriesCount - 1]:= linea;
          Result:= Linea;
        end;
            */
        #endregion

        #region EVENTS LINES
        private void btnSumLines_Click(object sender, EventArgs e)
        {
            try
            {
                if (chtPSD_L.Series.Count >= 2)
                    SumSeriesLine(chtPSD_L.Series[chtPSD_L.Series.Count - 2], chtPSD_L.Series[chtPSD_L.Series.Count - 1]);
                else
                    ABSMessageBox.Show("DEVI AVER DISEGNATO ALMENO DUE LINEE!!");
            }
            catch { }
        }

        private void chkSinAutoH_Click(object sender, EventArgs e)
        {
            txtSinH.Enabled = !chkSinH.Checked;
        }

        private void btnAddSinus_Click(object sender, EventArgs e)
        {
            float h = Convert.ToSingle(txtSinH.Text);
            float r = Convert.ToSingle(txtSinR.Text);
            double len = 0;
            double lenp = 0;
            float maxpercdiflen = Convert.ToSingle(txtSinMaxPercDifLen.Text);
            CalcolaValoriDiSinusoidePerGrafico(chtPSD_L.Series[chtPSD_L.Series.Count - 1], h, r, out len, out lenp, maxpercdiflen, 0);
            
        }

        private void btnAddSeriesPoint_Click(object sender, EventArgs e)
        {
            try
            {
                string z = cbColValZ.Text;
                if (string.IsNullOrEmpty(z))
                    z = "-" + z;
                string name = cbColValX.Text + "-" + cbColValY.Text + z;
                AddSeries(this.DATA_RESULTS, name, cbColValX.Text, cbColValY.Text, cbColValZ.Text, SeriesChartType.Point, chtPSD_P);
            }
            catch (Exception ex)
            {
                ABSMessageBox.Show("ERRORE: " + ex.Message);
            }
        }

        private List<Tuple<float, float>> CurrentSerVal;
        private Series CurrentSeries = new Series();
        private int CurrentIdxVal = 0;
        private int CurrentMaxIdx = 0;
        private void btnPlay_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            if (tcChrts.SelectedIndex == 0)
            {
                this.CurrentSeries = chtPSD_L.Series[chtPSD_L.Series.Count - 1];
            }
            else
            if (tcChrts.SelectedIndex == 1)
            {
                //Series s = uchtPSD.Series[uchtPSD.Series.Count - 1];
            }
            else
            if (tcChrts.SelectedIndex == 2)
            {
                this.CurrentSeries = chtPSD_P.Series[chtPSD_P.Series.Count - 1];
            }

            this.CurrentSerVal = GetSeriesPoints(this.CurrentSeries);
            this.CurrentMaxIdx = this.CurrentSerVal.Count;
            this.CurrentSeries.Points.Clear();

            if (Convert.ToInt32(timer1.Tag) == 0)
            {
                this.CurrentIdxVal = 0;
                timer1.Interval = Convert.ToInt32(txtMsecTimer.Text);
                timer1.Start();
                timer1.Tag = 1;
            }
            else
            {
                timer1.Stop();
                timer1.Tag = 0;
            }
            this.Cursor = Cursors.Default;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            bool endplay = false;
          
            if (this.CurrentIdxVal < this.CurrentMaxIdx)
            { 
                this.CurrentSeries.Points.AddXY(CurrentSerVal[CurrentIdxVal].Item1, CurrentSerVal[CurrentIdxVal].Item2);
                this.CurrentIdxVal++;
            }
            else endplay = true;

            if (endplay)
            {
                timer1.Stop();
                timer1.Tag = 0;
                this.CurrentIdxVal = 0;
            }
        }

        private static List<Tuple<float, float>> GetSeriesPoints(Series s1)
        {
            List<Tuple<float, float>> lt = new List<Tuple<float, float>>();
            foreach (DataPoint p in s1.Points)
            {
                Tuple<float, float> t = new Tuple<float, float>((float)p.XValue, (float)p.YValues.First());
                lt.Add(t);
            }
            return lt;
        }
        #endregion

        #region PRINT CHART

        private Excel.Application XlApp;
        private Excel.Workbook XlWorkBook;

        public void InitExcelObjects(Excel.Application xlApp)
        {
            object misValue = System.Reflection.Missing.Value;
            if (xlApp == null)
                this.XlApp = new Excel.Application();
            else this.XlApp = xlApp;

            // attenzione: parte da indice 1
            if (xlApp.Workbooks.Count > 0)
                this.XlWorkBook = xlApp.Workbooks[1];

            if (this.XlWorkBook == null)
                this.XlWorkBook = this.XlApp.Workbooks.Add(misValue);
        }

        public void ExportChartXls(List<PSD_RES_TOT> res, string colNameX, string colNameY, string colNameZ, string linename, int numline, bool releaseGlobalObj)
        {

            Excel.Worksheet xlWorkSheet;

            object misValue = System.Reflection.Missing.Value;

            if (this.XlApp == null)
                this.XlApp = new Excel.Application();

            if (this.XlWorkBook == null)
                this.XlWorkBook = XlApp.Workbooks.Add(misValue);

            //if (string.IsNullOrEmpty(linename))
            //    linename = this.TITLE_CHART;

            bool exists = false;
            //if (string.IsNullOrEmpty(filename))
            string filename = GetFileNameXls(linename, out exists);

            if (numline > this.XlWorkBook.Worksheets.Count)
                xlWorkSheet = (Excel.Worksheet)this.XlWorkBook.Worksheets.Add();
            else
                xlWorkSheet = (Excel.Worksheet)this.XlWorkBook.Worksheets.get_Item(numline);

            if (!string.IsNullOrEmpty(linename))
                xlWorkSheet.Name = linename;
            else xlWorkSheet.Name = "Foglio " + numline.ToString();

            DataTable dt = Utils.ToDataTable<PSD_RES_TOT>(res);
            try
            {
                //add data //
                int idxY = 1;//(2 * (numline - 1)) + 1;
                int idxX = 2;
                xlWorkSheet.Cells[1, 1] = "X val";
                xlWorkSheet.Cells[1, 2] = "Y val";
                xlWorkSheet.Cells[1, 3] = "Z val";

                int numpoints = res.Count;

                foreach (DataRow r in dt.Rows)
                {
                    xlWorkSheet.Cells[idxX, idxY] = Convert.ToSingle(r[colNameX]);
                    xlWorkSheet.Cells[idxX, idxY + 1] = Convert.ToSingle(r[colNameY]);
                    if(!string.IsNullOrEmpty(colNameZ)) xlWorkSheet.Cells[idxX, idxY + 2] = Convert.ToSingle(r[colNameZ]);
                    else xlWorkSheet.Cells[idxX, idxY + 2] = 0;         
                }

                Excel.Range chartRange;

                int wdtchart = 35 * numpoints;
                Excel.ChartObjects xlCharts = (Excel.ChartObjects)xlWorkSheet.ChartObjects(Type.Missing);
                Excel.ChartObject myChart = (Excel.ChartObject)xlCharts.Add(150, 120, wdtchart, 150);
                Excel.Chart chartPage = myChart.Chart;

                chartRange = xlWorkSheet.get_Range("A1", "B" + (numpoints + 1).ToString());
                if (!string.IsNullOrEmpty(colNameZ))  chartRange = xlWorkSheet.get_Range("A1", "C" + (numpoints + 1).ToString());
                chartPage.SetSourceData(chartRange, misValue);
                //chartPage.ChartType = Excel.XlChartType.xlColumnClustered;
                chartPage.ChartType = Excel.XlChartType.xlLine;
                if (!string.IsNullOrEmpty(colNameZ)) chartPage.ChartType = Excel.XlChartType.xl3DLine;

                #region LAYOUT CHART XLS
                chartPage.ChartTitle.Caption = linename;
                chartPage.AutoScaling = true;
                chartPage.ApplyDataLabels(Excel.XlDataLabelsType.xlDataLabelsShowValue);
                chartPage.ApplyLayout(5);

                myChart.Interior.Color = ColorTranslator.ToOle(Color.LightSkyBlue);
                #endregion

                var yAxis = (Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary);
                yAxis.HasTitle = true;
                yAxis.AxisTitle.Text = colNameY;
                yAxis.AxisTitle.Orientation = Excel.XlOrientation.xlUpward;

                //string filenamexls = GetFileNameXls(filename, out exists);
                //if(!exists) this.XlWorkBook.SaveAs(filename, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                //else this.XlWorkBook.Save();

                ReleaseObject(xlWorkSheet);
                if (releaseGlobalObj)
                {
                    //string filenamexls = GetFileNameXls(linename);
                    CloseExcelObjects(filename);
                }
            }
            catch (Exception ex)
            {
                ABSMessageBox.Show("Excel file ERROR: " + ex.Message);
                ReleaseObject(this.XlWorkBook);
                ReleaseObject(this.XlApp);
            }
        }

        public string GetFileNameXls(string linename, out bool exists)
        {
            string datestr =
                                DateTime.Now.Year.ToString() +
                                DateTime.Now.Month.ToString() +
                                DateTime.Now.Day.ToString() + "_" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
            string dir = ABSIniFile.GetReportFolderFromIni();
            if (string.IsNullOrEmpty(dir))
                dir = "C:\\Blooming\\L2\\Printout\\";

            string namexls = dir + linename + "_" + datestr + ".xls";
            exists = false;
            try
            {
                if (!System.IO.Directory.Exists(dir))
                    System.IO.Directory.CreateDirectory(dir);

                if (System.IO.File.Exists(namexls))
                    //System.IO.File.Delete(namexls);
                    exists = true;
            }
            catch (Exception ex)
            {
                ABSMessageBox.Show("Excel file ERROR: " + ex.Message);
            }
            return namexls;
        }

        public void CloseExcelObjects(string filenamexls)
        {
            try
            {
                object misValue = System.Reflection.Missing.Value;
                this.XlWorkBook.SaveAs(filenamexls, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                this.XlWorkBook.Close(true, misValue, misValue);
                this.XlApp.Quit();

                //ReleaseObject(xlWorkSheet);
                ReleaseObject(this.XlWorkBook);
                ReleaseObject(this.XlApp);

                System.Diagnostics.Process newProcess = null;
                newProcess = System.Diagnostics.Process.Start(filenamexls);
            }
            catch { }
        }

        private void ReleaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
        #endregion

        private void btnExportXls_Click(object sender, EventArgs e)
        {
            try
            {
                string z = cbColValZ.Text;
                if (string.IsNullOrEmpty(z))
                    z = "-" + z;
                string name = cbColValX.Text + "-" + cbColValY.Text + z + "  " + this.DATA_RESULTS.First().DATA_EVENTO_DA.ToShortDateString() + "-" + this.DATA_RESULTS.Last().DATA_EVENTO_A.ToShortDateString();
                ExportChartXls(this.DATA_RESULTS, cbColValX.Text, cbColValY.Text, cbColValZ.Text, name, 1 ,true);
            }
            catch (Exception ex)
            {
                ABSMessageBox.Show("ERRORE: " + ex.Message);
            }
           
        }
    }
}
