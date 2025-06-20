import pandas as pd
import re
import sys
import json
import os

class Config:
    ATTEMPTS = 2

class FilePathConfig:
    DATA_DIR = r"D:\new\Watts_Guard\Grad_Project\mnt\data"

    APPLIANCE_FILE_PATH = os.path.join(DATA_DIR, "Electrical appliances.xlsx")
    DEVICE_FILE_PATH = os.path.join(DATA_DIR, "washing machine.xlsx")
    AIR_CONDITIONERS_PATH = os.path.join(DATA_DIR, "Air Conditioners(2).xlsx")
    ELECTRICAL_KATTEL_PATH = os.path.join(DATA_DIR, "Electrical kattel.xlsx")
    WATER_HEATER_PATH = os.path.join(DATA_DIR, "water heater.xlsx")
    AIRFRYER_PATH = os.path.join(DATA_DIR, "Airfryer.xlsx")
    VACCUM_CLEANERS_PATH = os.path.join(DATA_DIR, "vaccum cleaners(2).xlsx")
    DISHWASHER_PATH = os.path.join(DATA_DIR, "dishwasher.xlsx")
    ELECTRICAL_HEATER_PATH = os.path.join(DATA_DIR, "electrical heater.xlsx")
    STOVE_PATH = os.path.join(DATA_DIR, "electrical heater.xlsx")
    MICROWAVE_OVEN_PATH = os.path.join(DATA_DIR, "Microwave & Oven.xlsx")

DEFAULT_DEVICES = {
    "Television": 0.12,
    "Fans (2 units)": 0.17,
    "Wi-Fi Router": 0.010,
    "Lighting (LED Bulbs × 8)": 0.086,
    "Chargers & Small Devices": 0.025
}

class WaterHeater:
    def __init__(self, file_path):
        self.file_path = file_path
        self.data = pd.read_excel(self.file_path)
        self.data.columns = self.data.columns.str.strip()
        if "Power" in self.data.columns:
            self.data = self.data.rename(columns={"Power": "Electric Power"})
        if "capacity" in self.data.columns:
            self.data = self.data.rename(columns={"capacity": "Capacity"})
        self.attempts = Config.ATTEMPTS
    
    def get_device_details(self, device):
        device = str(device)
        result = self.data[self.data['Model Name'].str.contains(device, na=False, case=False)]
        if result.empty:
            return None  
        output = ""
        for _, row in result.iterrows():
            for column, value in row.items():
                unit = ""
                if column.lower() == "capacity":
                    unit = " L"
                elif column.lower() == "electric power":
                    unit = " kW"
                output += f"{column}: {value}{unit}\n"
            output += "\n"
        return output.strip()

class Refrigerator:
    def __init__(self, file_path):
        self.file_path = file_path
        self.data = pd.read_excel(self.file_path)
        self.data.columns = self.data.columns.str.strip()
        if "Power" in self.data.columns:
            self.data = self.data.rename(columns={"Power": "Electric Power"})
        if "capacity" in self.data.columns:
            self.data = self.data.rename(columns={"capacity": "Capacity"})
        self.attempts = Config.ATTEMPTS
        
    def get_device_details(self, model_name):
        model_name = str(model_name)
        result = self.data[self.data['Model Name'].str.contains(model_name, na=False, case=False)]
        if result.empty:
            return None
        output = ""
        for _, row in result.iterrows():
            for column, value in row.items():
                unit = ""
                if column.lower() == "capacity":
                    unit = " L"
                elif column.lower() == "electric power":
                    unit = " kW"
                output += f"{column}: {value}{unit}\n"
            output += "\n"
        return output.strip()

class WashingMachine:
    def __init__(self, file_path):
        self.file_path = file_path
        self.data = pd.read_excel(self.file_path)
        self.data.columns = self.data.columns.str.strip()
        if "Power" in self.data.columns:
            self.data = self.data.rename(columns={"Power": "Electric Power"})
        if "capacity" in self.data.columns:
            self.data = self.data.rename(columns={"capacity": "Capacity"})
        self.attempts = Config.ATTEMPTS
    
    def get_device_details(self, device_name):
        device_name = str(device_name)
        result = self.data[self.data['Model Name'] == device_name]
        if result.empty:
            return None  
        output = ""
        for _, row in result.iterrows():
            for column, value in row.items():
                unit = ""
                if column.lower() == "capacity":
                    unit = " L"
                elif column.lower() == "electric power":
                    unit = " kWh"
                output += f"{column}: {value}{unit}\n"
            output += "\n"
        return output.strip()

class AirConditioners:
    def __init__(self, file_path):
        self.file_path = file_path
        self.data = pd.read_excel(self.file_path)
        self.data.columns = self.data.columns.str.strip()
        if "Power" in self.data.columns:
            self.data = self.data.rename(columns={"Power": "Electric Power"})
        if "capacity" in self.data.columns:
            self.data = self.data.rename(columns={"capacity": "Capacity"})
        self.attempts = Config.ATTEMPTS
    
    def get_device_details(self, device_name):
        device_name = str(device_name)
        result = self.data[self.data['Model Name'] == device_name]
        if result.empty:
            return None  
        output = ""
        for _, row in result.iterrows():
            for column, value in row.items():
                unit = ""
                if column.lower() == "capacity":
                    unit = " H"
                elif column.lower() == "electric power":
                    unit = " kWh"
                output += f"{column}: {value}{unit}\n"
            output += "\n"
        return output.strip()

class ElectricalKattel:
    def __init__(self, file_path):
        self.file_path = file_path
        self.data = pd.read_excel(self.file_path)
        self.data.columns = self.data.columns.str.strip()
        if "Power" in self.data.columns:
            self.data = self.data.rename(columns={"Power": "Electric Power"})
        if "capacity" in self.data.columns:
            self.data = self.data.rename(columns={"capacity": "Capacity"})
        self.attempts = Config.ATTEMPTS
    
    def get_device_details(self, device):
        device = str(device)
        result = self.data[self.data['Model Name'].str.contains(device, na=False, case=False)]
        if result.empty:
            return None  
        output = ""
        for _, row in result.iterrows():
            for column, value in row.items():
                unit = ""
                if column.lower() == "capacity":
                    unit = " L"
                elif column.lower() == "electric power":
                    unit = " W"
                output += f"{column}: {value}{unit}\n"
            output += "\n"
        return output.strip()

class Airfryer:
    def __init__(self, file_path):
        self.file_path = file_path
        self.data = pd.read_excel(self.file_path)
        self.data.columns = self.data.columns.str.strip()
        if "Power" in self.data.columns:
            self.data = self.data.rename(columns={"Power": "Electric Power"})
        if "capacity" in self.data.columns:
            self.data = self.data.rename(columns={"capacity": "Capacity"})
        self.attempts = Config.ATTEMPTS  
    
    def get_device_details(self, device):
        device = str(device)
        result = self.data[self.data['Model Name'].str.contains(device, na=False, case=False)]
        if result.empty:
            return None  
        output = ""
        for _, row in result.iterrows():
            for column, value in row.items():
                unit = ""
                if column.lower() == "capacity":
                    unit = " L"
                elif column.lower() == "electric power":
                    unit = " W"
                output += f"{column}: {value}{unit}\n"
            output += "\n"
        return output.strip()

class VaccumCleaners:
    def __init__(self, file_path):
        self.file_path = file_path
        self.data = pd.read_excel(self.file_path)
        self.data.columns = self.data.columns.str.strip()
        if "Power" in self.data.columns:
            self.data = self.data.rename(columns={"Power": "Electric Power"})
        if "capacity" in self.data.columns:
            self.data = self.data.rename(columns={"capacity": "Capacity"})
        self.attempts = Config.ATTEMPTS  
    
    def get_device_details(self, device):
        device = str(device)
        result = self.data[self.data['Model Name'].str.contains(device, na=False, case=False)]
        if result.empty:
            return None  
        output = ""
        for _, row in result.iterrows():
            for column, value in row.items():
                unit = ""
                if column.lower() == "capacity":
                    unit = " L"
                elif column.lower() == "electric power":
                    unit = " W"
                output += f"{column}: {value}{unit}\n"
            output += "\n"
        return output.strip()

class Dishwasher:
    def __init__(self, file_path):
        self.file_path = file_path
        self.data = pd.read_excel(self.file_path)
        self.data.columns = self.data.columns.str.strip()
        if "Power" in self.data.columns:
            self.data = self.data.rename(columns={"Power": "Electric Power"})
        if "Number of places" in self.data.columns:
            self.data = self.data.rename(columns={"Number of places": "Capacity"})
        self.attempts = Config.ATTEMPTS  
    
    def get_device_details(self, device):
        device = str(device)
        result = self.data[self.data['Model Name'].str.contains(device, na=False, case=False)]
        if result.empty:
            return None  
        output = ""
        for _, row in result.iterrows():
            for column, value in row.items():
                unit = ""
                if column.lower() == "capacity":
                    unit = " places"
                elif column.lower() == "electric power":
                    unit = " kW"
                output += f"{column}: {value}{unit}\n"
            output += "\n"
        return output.strip()

class Stove:
    def __init__(self, file_path):
        self.file_path = file_path
        self.data = pd.read_excel(self.file_path, sheet_name="Sheet3")
        self.data.columns = self.data.columns.str.strip()
        if "Power" in self.data.columns:
            self.data = self.data.rename(columns={"Power": "Electric Power"})
        if "Number of places" in self.data.columns:
            self.data = self.data.rename(columns={"Number of places": "Capacity"})
        self.attempts = Config.ATTEMPTS  
    
    def get_device_details(self, device):
        device = str(device)
        result = self.data[self.data['Model Name'].str.contains(device, na=False, case=False)]
        if result.empty:
            return None  
        output = ""
        for _, row in result.iterrows():
            for column, value in row.items():
                unit = ""
                if column.lower() == "capacity":
                    unit = " places"
                elif column.lower() == "electric power":
                    unit = " kW"
                output += f"{column}: {value}{unit}\n"
            output += "\n"
        return output.strip()

class SteamIrons:
    def __init__(self, file_path):
        self.file_path = file_path
        self.data = pd.read_excel(self.file_path, sheet_name="Sheet1")
        self.data.columns = self.data.columns.str.strip()
        if "Power" in self.data.columns:
            self.data = self.data.rename(columns={"Power": "Electric Power"})
        if "Number of places" in self.data.columns:
            self.data = self.data.rename(columns={"Number of places": "Capacity"})
        self.attempts = Config.ATTEMPTS  
    
    def get_device_details(self, device):
        device = str(device)
        result = self.data[self.data['Model Name'].str.contains(device, na=False, case=False)]
        if result.empty:
            return None 
        output = ""
        for _, row in result.iterrows():
            for column, value in row.items():
                unit = ""
                if column.lower() == "capacity":
                    unit = " N"
                elif column.lower() == "electric power":
                    unit = " W"
                output += f"{column}: {value}{unit}\n"
            output += "\n"
        return output.strip()

class Oven:
    def __init__(self, file_path):
        self.file_path = file_path
        self.data = pd.read_excel(self.file_path, sheet_name="Sheet1")
        self.data.columns = self.data.columns.str.strip()
        if "Power" in self.data.columns:
            self.data = self.data.rename(columns={"Power": "Electric Power"})
        if "Number of places" in self.data.columns:
            self.data = self.data.rename(columns={"Number of places": "Capacity"})
        self.attempts = Config.ATTEMPTS  
    
    def get_device_details(self, device):
        device = str(device)
        result = self.data[self.data['Model Name'].str.contains(device, na=False, case=False)]
        if result.empty:
            return None  
        output = ""
        for _, row in result.iterrows():
            for column, value in row.items():
                unit = ""
                if column.lower() == "capacity":
                    unit = " L"
                elif column.lower() == "electric power":
                    unit = " kW"
                output += f"{column}: {value}{unit}\n"
            output += "\n"
        return output.strip()

class Microwave:
    def __init__(self, file_path):
        self.file_path = file_path
        self.data = pd.read_excel(self.file_path, sheet_name="Sheet2")
        self.data.columns = self.data.columns.str.strip()
        if "Power" in self.data.columns:
            self.data = self.data.rename(columns={"Power": "Electric Power"})
        if "capacity" in self.data.columns:
            self.data = self.data.rename(columns={"capacity": "Capacity"})
        self.attempts = Config.ATTEMPTS  
    
    def get_device_details(self, device):
        device = str(device)
        result = self.data[self.data['Model Name'].str.contains(device, na=False, case=False)]
        if result.empty:
            return None  
        output = ""
        for _, row in result.iterrows():
            for column, value in row.items():
                unit = ""
                if column.lower() == "capacity":
                    unit = " L"
                elif column.lower() == "electric power":
                    unit = " kW"
                output += f"{column}: {value}{unit}\n"
            output += "\n"
        return output.strip()

def clean_model_name(name):
    return str(name).strip().lower().replace(" ", "").replace(".", "")

class UserInputHandler:
    results = []

    @staticmethod
    def get_valid_input(db, model):
        model = str(model)
        attempts = Config.ATTEMPTS
        
        while attempts > 0:
            details = db.get_device_details(model)
            if details:
                details_dict = {"Model Name": model}
                for line in details.split("\n"):
                    if ": " in line:
                        key, val = line.split(": ", 1)
                        details_dict[key.strip()] = val.strip()
                UserInputHandler.results.append(details_dict)
                return details
            attempts -= 1
        return None

def read_device_input(db, answers, idx):
    if idx >= len(answers):
        return idx
    
    has_device = str(answers[idx]).strip().lower()
    idx += 1
    
    if has_device == "yes":
        if idx >= len(answers):
            raise ValueError(f"Missing count for {db.__class__.__name__}")
        
        try:
            count = int(answers[idx])
        except ValueError:
            raise ValueError(f"Invalid count format for {db.__class__.__name__}")
        
        idx += 1
        
        for _ in range(count):
            if idx >= len(answers):
                raise ValueError(f"Missing model name for {db.__class__.__name__}")
            
            model = answers[idx]
            UserInputHandler.get_valid_input(db, model)
            idx += 1
    
    return idx

class PowerSummary:
    def __init__(self, searched_models, *databases):
        self.power_data = []
        
        for db in databases:
            db_name = db.__class__.__name__  
            
            for _, row in db.data.iterrows():
                model = row.get("Model Name", "Unknown")
                power = row.get("Electric Power", None)  
                
                if model in searched_models and power is not None:
                    self.power_data.append({
                        "Category": db_name,
                        "Electric Power": power,
                        "Model name": model
                    })
        
        for device, percentage in DEFAULT_DEVICES.items():
            self.power_data.append({
                "Category": device, 
                "Electric Power": percentage,
                "Model name": "DEFAULT_DEVICES" 
            })

    def get_power_summary(self):
        return pd.DataFrame(self.power_data)

class ElectricityBillCalculator:
    TARIFFS = [
        (50, 68),    
        (100, 78),   
        (200, 95),   
        (350, 155),  
        (650, 195),  
        (1000, 210), 
        (float('inf'), 230)  
    ]
    
    @staticmethod
    def calculate_consumption(amount):
        remaining_amount = amount * 100
        total_consumption = 0
        previous_limit = 0
        
        for limit, price in ElectricityBillCalculator.TARIFFS:
            slab_capacity = limit - previous_limit
            cost_of_slab = slab_capacity * price
            
            if remaining_amount >= cost_of_slab:
                total_consumption += slab_capacity
                remaining_amount -= cost_of_slab
            else:
                total_consumption += remaining_amount / price
                break
            
            previous_limit = limit
        
        if total_consumption > 1000:
            total_consumption = (amount * 100) / 230
        
        return total_consumption

def format_hours(hours_decimal):
    hours = int(hours_decimal)  
    minutes = round((hours_decimal - hours) * 60)  
    return f"{hours}h {minutes}m"

def distribute_power_consumption(total_consumption, power_df):
    CATEGORY_PERCENTAGES = {
        "AirConditioners": .296,
        "WaterHeater": .04,
        "Stove": .08,
        "Oven": .06,
        "Dishwasher": .04,
        "WashingMachine": .03,
        "ElectricalKattel": .03,
        "SteamIrons": .02,
        "Airfryer": .014,
        "Microwave": .01,
        "VaccumCleaners": .01,
        "Television": .024,
        "Fans (2 units)": .03,
        "Wi-Fi Router": .016,
        "Lighting (LED Bulbs × 8)": .02,
        "Chargers & Small Devices": .045
    }

    power_df["Category Percentage"] = power_df["Category"].map(CATEGORY_PERCENTAGES)
    power_df["Devices Count"] = power_df.groupby("Category")["Category"].transform("count")
    power_df["Category Percentage"] = power_df["Category Percentage"] / power_df["Devices Count"]
    power_df = power_df.groupby(["Category", "Model name"], as_index=False).agg({
        "Electric Power": "sum",  
        "Category Percentage": "first"
    })
    excluded_categories = ["Wi-Fi Router", "Chargers & Small Devices", "Refrigerator", "WaterHeater"]

    if "Refrigerator" in power_df["Category"].values:
        fridge_indices = power_df[power_df["Category"] == "Refrigerator"].index
        for index in fridge_indices:
            fridge_power = power_df.loc[index, "Electric Power"]
            fridge_percentage = fridge_power / total_consumption
            power_df.loc[index, "Category Percentage"] = fridge_percentage
            power_df.loc[index, "Monthly Consumption (kWh)"] = fridge_power

    total_percentage = power_df["Category Percentage"].sum()
    remaining_percentage = max(0, 1 - total_percentage)
    adjustable_mask = ~power_df["Category"].isin(excluded_categories)
    num_adjustable_categories = adjustable_mask.sum()

    if num_adjustable_categories > 0:
        power_df.loc[adjustable_mask, "Category Percentage"] += remaining_percentage / num_adjustable_categories

    power_df["Category Percentage"] = (power_df["Category Percentage"] * 100).round(2).astype(str) + "%"
    power_df["Category Percentage"] = power_df["Category Percentage"].str.rstrip('%').astype(float) / 100
    power_df["Monthly Consumption (kWh)"] = (power_df["Category Percentage"] * total_consumption).round(2)
    power_df["Daily Hours"] = (power_df["Monthly Consumption (kWh)"] / (power_df["Electric Power"] * 30))
    power_df.loc[power_df["Category"] == "Refrigerator", "Daily Hours"] = 24
    power_df["Daily Hours"] = power_df["Daily Hours"].apply(lambda x: min(max(x, 0), 24)).round(2)
    power_df["Daily Hours"] = power_df["Daily Hours"].apply(format_hours)
    
    return power_df[["Category", "Model name", "Monthly Consumption (kWh)", "Daily Hours"]]

if __name__ == "__main__":
    try:
        raw_input = sys.stdin.read()
        user_inputs = json.loads(raw_input)

        index = 0
        amount = float(user_inputs[index])
        index += 1

        appliance_db = Refrigerator(FilePathConfig.APPLIANCE_FILE_PATH)
        device_db = WashingMachine(FilePathConfig.DEVICE_FILE_PATH)
        air_conditioners_db = AirConditioners(FilePathConfig.AIR_CONDITIONERS_PATH)
        electrical_kattel_db = ElectricalKattel(FilePathConfig.ELECTRICAL_KATTEL_PATH)
        water_heater_db = WaterHeater(FilePathConfig.WATER_HEATER_PATH)
        airfryer_db = Airfryer(FilePathConfig.AIRFRYER_PATH)
        vaccum_cleaners_db = VaccumCleaners(FilePathConfig.VACCUM_CLEANERS_PATH)
        dishwasher_db = Dishwasher(FilePathConfig.DISHWASHER_PATH)
        stove_db = Stove(FilePathConfig.STOVE_PATH)
        steam_irons_db = SteamIrons(FilePathConfig.STOVE_PATH)
        oven_db = Oven(FilePathConfig.MICROWAVE_OVEN_PATH)
        microwave_db = Microwave(FilePathConfig.MICROWAVE_OVEN_PATH)

        total_consumption = ElectricityBillCalculator.calculate_consumption(amount)

        index = read_device_input(appliance_db, user_inputs, index)
        index = read_device_input(device_db, user_inputs, index)
        index = read_device_input(air_conditioners_db, user_inputs, index)
        index = read_device_input(electrical_kattel_db, user_inputs, index)
        index = read_device_input(water_heater_db, user_inputs, index)
        index = read_device_input(airfryer_db, user_inputs, index)
        index = read_device_input(vaccum_cleaners_db, user_inputs, index)
        index = read_device_input(dishwasher_db, user_inputs, index)
        index = read_device_input(stove_db, user_inputs, index)
        index = read_device_input(steam_irons_db, user_inputs, index)
        index = read_device_input(oven_db, user_inputs, index)
        index = read_device_input(microwave_db, user_inputs, index)

        df = pd.DataFrame(UserInputHandler.results)
        if "Model Name" not in df.columns or df.empty:
            print(json.dumps({
                "success": False,
                "error": "No valid models were found or 'Model Name' is missing."
            }, ensure_ascii=False))
            sys.exit()

        searched_models = df["Model Name"].tolist()
        power_summary = PowerSummary(searched_models, device_db, air_conditioners_db,
                                    appliance_db, electrical_kattel_db, water_heater_db,
                                    airfryer_db, vaccum_cleaners_db, dishwasher_db,
                                    stove_db, steam_irons_db, oven_db, microwave_db)
        power_df = power_summary.get_power_summary()
        power_df = distribute_power_consumption(total_consumption, power_df)

        output = {
            "success": True,
            "summary": power_df.to_dict(orient="records"),
            "estimated_bill_range": f"{amount} to {amount + 50}",
            "message": "Avoid using devices from 6 to 10 PM"
        }

        print(json.dumps(output, ensure_ascii=False))

    except Exception as e:
        print(json.dumps({
            "success": False,
            "error": "Python script execution failed.",
            "details": str(e)
        }, ensure_ascii=False))