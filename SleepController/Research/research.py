#%%
from bokeh.plotting import figure, show, output_file
from bokeh.models import ColumnDataSource, CategoricalColorMapper
from bokeh.transform import factor_cmap, linear_cmap, log_cmap

import pandas as pd
#%%
input_path = r'C:\Users\Spaier\Projects\VSU\HMI\SleepController\Domain.Test\bin\Debug\netcoreapp2.2\results.csv'
output_path = r'charts.html'
df = pd.read_csv(input_path)
output_file(r'research.html')
#%%
data = {
    'is_detected': df['IsDetected'],
    'time': range(df.shape[0]),
    'value': df['Value'],
    'average': df['Average'],
    'floating_average': df['FloatingAverage'],
}
#%%
source = ColumnDataSource(data)
p1 = figure(plot_height=730, plot_width=1500)
p1.circle('time', 'value', legend='Value', color=linear_cmap('is_detected', ["red", "yellow"], 0, 1), size=0.1, source=source)
p1.line('time', 'average', color='blue', legend='Average', source=source)
p1.line('time', 'floating_average', color='green', legend='Floating Average', source=source)
show(p1)
