import pandas as pd
import matplotlib.pyplot as plt
import seaborn as sns
import os

# Load data from CSV (updated path to match project structure)
df = pd.read_csv("mnt/data/devices_data_egypt.csv")  # Adjusted path based on project structure

# Count devices per city
device_counts = df['city'].value_counts().reset_index()
device_counts.columns = ['city', 'count']

# Filter to include only specified cities
cities = ['Cairo', 'Alexandria', 'Aswan', 'Giza', 'Mansoura']
device_counts = device_counts[device_counts['city'].isin(cities)]

# Create bar plot
plt.figure(figsize=(10, 6))
sns.set_style("whitegrid")
sns.barplot(x='count', y='city', data=device_counts, hue='city', palette='viridis', legend=False)

plt.title("Device Count by City in Egypt", fontsize=16, fontweight='bold')
plt.xlabel("Number of Devices", fontsize=14)
plt.ylabel("City", fontsize=14)
plt.tight_layout()

# Create output directory if it doesn't exist
output_dir = "wwwroot/charts"
os.makedirs(output_dir, exist_ok=True)

# Save the chart
plt.savefig(os.path.join(output_dir, "chart_devices_by_city.png"))
plt.close()  # Close the figure to free memory