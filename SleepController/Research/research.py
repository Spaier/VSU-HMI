#%%
from bokeh.plotting import figure, show, output_file

import pandas as pd
#%%
input_path = r'C:\Users\Spaier\Projects\VSU\HMI\EegSleepController\SleepController\Domain.Test\bin\Debug\netcoreapp2.2\results.csv'
output_path = r'charts.html'
df = pd.read_csv(input_path)
output_file(r'research.html')
#%%
time_grid = range(df.shape[0])
#%%
p1 = figure(plot_height=730, plot_width=1500)
p1.line(time_grid, df['Value'], color='red', legend='Value')
p1.line(time_grid, df['Average'], color='blue', legend='Average')
p1.line(time_grid, df['FloatingAverage'], color='green', legend='Floating Average')
p1.line(time_grid, df['IsClosed'] * df['Value'], color='yellow', legend='Is Closed')
show(p1)
#%%
