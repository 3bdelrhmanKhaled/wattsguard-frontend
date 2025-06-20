import pandas as pd
import sys
import json
import os

DATA_DIR = r"D:\new\Watts_Guard\Grad_Project\mnt\data"

APPLIANCE_FILE_PATH = os.path.join(DATA_DIR, "Electrical appliances.xlsx")
DEVICE_FILE_PATH = os.path.join(DATA_DIR, "washing machine.xlsx")
ELECTRIC_HEATER_PATH = os.path.join(DATA_DIR, "electrical heater.xlsx")
ELECTRICAL_KATTEL_PATH = os.path.join(DATA_DIR, "Electrical kattel.xlsx")
WATER_HEATER_PATH = os.path.join(DATA_DIR, "water heater.xlsx")
AIRFRYER_PATH = os.path.join(DATA_DIR, "Airfryer.xlsx")
VACCUM_CLEANERS_PATH = os.path.join(DATA_DIR, "vaccum cleaners(2).xlsx")
DISHWASHER_PATH = os.path.join(DATA_DIR, "dishwasher.xlsx")
MICROWAVE_OVEN_PATH = os.path.join(DATA_DIR, "Microwave & Oven.xlsx")

# القائمة النهائية بدون تكرار
file_paths = [
    APPLIANCE_FILE_PATH,
    DEVICE_FILE_PATH,
    ELECTRIC_HEATER_PATH,
    ELECTRICAL_KATTEL_PATH,
    WATER_HEATER_PATH,
    AIRFRYER_PATH,
    VACCUM_CLEANERS_PATH,
    DISHWASHER_PATH,
    MICROWAVE_OVEN_PATH
]

# تنظيف الموديل المطلوب
model_name = sys.argv[1].strip().lower().replace(" ", "").replace(".", "")

def clean_model_column(df):
    df.columns = df.columns.str.strip()
    if "Model Name" not in df.columns:
        return None
    df["Model_Clean"] = df["Model Name"].str.strip().str.lower().str.replace(" ", "", regex=False).str.replace(".", "", regex=False)
    return df

# البحث في كل الملفات
for path in file_paths:
    if not os.path.exists(path):
        continue
    try:
        df = pd.read_excel(path)
        df = clean_model_column(df)
        if df is None:
            continue
        result = df[df["Model_Clean"] == model_name]
        if not result.empty:
            row = result.iloc[0].drop("Model_Clean")
            data = {k: str(v) for k, v in row.to_dict().items()}
            print(json.dumps({"success": True, "model_details": data, "source_file": os.path.basename(path)}))
            sys.exit(0)
    except Exception as e:
        continue  # تجاهل أي ملف يسبب مشكلة

# إذا لم يتم العثور على الموديل
print(json.dumps({"success": False, "error": "Model not found"}))