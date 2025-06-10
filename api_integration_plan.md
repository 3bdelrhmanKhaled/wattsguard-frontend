# خطة تكامل واجهة المستخدم مع واجهة برمجة التطبيقات (API)

## نظرة عامة على التكامل

سيتم ربط واجهة المستخدم (Frontend) بواجهة برمجة التطبيقات (Backend API) باستخدام طلبات HTTP من خلال JavaScript. سنستخدم Fetch API لإرسال واستقبال البيانات من نقاط النهاية (Endpoints) المختلفة.

## نقاط النهاية الرئيسية (API Endpoints)

1. **المصادقة (Authentication)**
   - `POST http://wattsguardak4529.runasp.net/api/Account/Login`
   - `POST http://wattsguardak4529.runasp.net/api/Account/RegisterAsUser`

2. **بيانات العداد (Counter Data)**
   - `GET http://wattsguardak4529.runasp.net/api/Counter/GetCounterData/guard2002`
   - `GET http://wattsguardak4529.runasp.net/api/Counter/IsUserThief/guard2002`
   - `POST http://wattsguardak4529.runasp.net/api/Counter/ProcessArduinoReading`

3. **حسابات الكهرباء (Electricity)**
   - `POST http://wattsguardak4529.runasp.net/api/Electricity/calculate?season=summer`
   - `GET http://wattsguardak4529.runasp.net/api/Electricity/model-details?model=EL1108866SL`

## خطة التكامل لكل صفحة

### 1. صفحة تسجيل الدخول (Login.html)

**نقطة النهاية**: `POST http://wattsguardak4529.runasp.net/api/Account/Login`

**البيانات المرسلة**:
```json
{
  "idNumber": "رقم الهوية",
  "password": "كلمة المرور"
}
```

**الاستجابة المتوقعة**:
```json
{
  "id": "معرف المستخدم",
  "idNumber": "رقم الهوية",
  "name": "اسم المستخدم",
  "email": "البريد الإلكتروني",
  "counterId": "معرف العداد",
  "token": "رمز المصادقة JWT",
  "expiration": "تاريخ انتهاء الصلاحية",
  "address": {
    "street": "الشارع",
    "region": "المنطقة",
    "city": "المدينة",
    "governorate": "المحافظة"
  }
}
```

**منطق التكامل**:
- إضافة مستمع حدث للزر "دخول"
- جمع بيانات المستخدم من حقول الإدخال
- إرسال طلب POST إلى نقطة النهاية
- تخزين رمز المصادقة JWT في التخزين المحلي (localStorage)
- إعادة توجيه المستخدم إلى الصفحة الرئيسية عند نجاح تسجيل الدخول
- عرض رسالة خطأ عند فشل تسجيل الدخول

### 2. صفحة إنشاء حساب (Signup.html)

**نقطة النهاية**: `POST http://wattsguardak4529.runasp.net/api/Account/RegisterAsUser`

**البيانات المرسلة**:
```json
{
  "idNumber": "رقم الهوية",
  "name": "الاسم",
  "password": "كلمة المرور",
  "email": "البريد الإلكتروني",
  "phone": "رقم الهاتف",
  "counterId": "معرف العداد",
  "address": {
    "street": "الشارع",
    "region": "المنطقة",
    "city": "المدينة",
    "governorate": "المحافظة"
  }
}
```

**الاستجابة المتوقعة**:
```
User registered successfully
```

**منطق التكامل**:
- تحديث وظيفة `submitForm()` الموجودة
- إرسال طلب POST إلى نقطة النهاية
- عرض رسالة نجاح وإعادة توجيه المستخدم إلى صفحة تسجيل الدخول عند نجاح التسجيل
- عرض رسالة خطأ عند فشل التسجيل

### 3. صفحة إضافة جهاز (Add-device2.html)

**نقطة النهاية**: `POST http://wattsguardak4529.runasp.net/api/Electricity/calculate?season=summer`

**البيانات المرسلة**:
```json
["2458", "yes", "1", "FNT-B400 BB", "yes", "2", "EL1094877SLV", "EL1087567SLV", ...]
```

**الاستجابة المتوقعة**:
```json
{
  "success": true,
  "summary": [
    {
      "Category": "WaterHeater",
      "Model_name": "TEEE-30MW",
      "Monthly Consumption (kWh)": 338.29,
      "Daily Hours": "7h 31m"
    },
    ...
  ]
}
```

**منطق التكامل**:
- تحديث وظيفة `sendToBackend()` الموجودة
- إضافة رمز المصادقة JWT إلى رأس الطلب
- معالجة البيانات المستلمة وعرضها في الجداول المناسبة

### 4. صفحات الإدارة (Admin Pages)

#### 4.1 صفحة موقع السرقة (thefe-direction.html)

**نقطة النهاية**: لا يوجد طلب API مباشر، تستخدم للتنقل فقط

**منطق التكامل**:
- إضافة التحقق من صلاحيات المستخدم (يجب أن يكون مسؤولاً)
- تحميل بيانات المحافظات من التخزين المحلي أو من طلب API منفصل

#### 4.2 صفحة عرض بيانات السرقة (newcopy.html)

**نقطة النهاية**: لا يوجد طلب API مباشر، تعرض بيانات الرسوم البيانية والخرائط

**منطق التكامل**:
- تحديث البيانات في الرسوم البيانية والخرائط من مصدر بيانات مركزي
- إضافة تحديث دوري للبيانات إذا لزم الأمر

#### 4.3 صفحة تفاصيل المنزل (last.html)

**نقطة النهاية**: 
- `GET http://wattsguardak4529.runasp.net/api/Counter/GetCounterData/guard2002`
- `GET http://wattsguardak4529.runasp.net/api/Counter/IsUserThief/guard2002`

**الاستجابة المتوقعة**:
```json
{
  "id": 1,
  "timeStamp": "2025-06-02T23:57:39.5",
  "reading": 56666,
  "flag": 1,
  "counterId": "c6007981-1320-4028-a023-0c250341fd0a",
  "isTheftReported": true,
  "address": {
    "street": "شارع 11",
    "region": "أحمد ماهر",
    "city": "المنصورة",
    "governorate": "الدقهلية"
  }
}
```

**منطق التكامل**:
- إضافة طلب GET لجلب بيانات العداد عند تحميل الصفحة
- عرض قراءة العداد الرسمية والفعلية
- إضافة وظائف للأزرار "تحذير المستخدم" و "إبلاغ عن السرقة"

## هيكل المشروع

```
wattsguard_frontend/
├── css/
│   └── style.css
├── js/
│   ├── api.js (وحدة التعامل مع API)
│   ├── auth.js (وحدة المصادقة)
│   ├── counter.js (وحدة بيانات العداد)
│   └── electricity.js (وحدة حسابات الكهرباء)
├── images/
├── Login.html
├── Signup.html
├── Home.html
├── Add-device2.html
├── thefe-direction.html
├── newcopy.html
├── newcopy2.html
├── last.html
├── admin-street.html
├── admin-points.html
├── admin-house.html
├── admin-covernment.html
└── Admin-home.html
```

## وحدات JavaScript المشتركة

### 1. وحدة API (api.js)

```javascript
// وحدة للتعامل مع طلبات API
const API_BASE_URL = 'http://wattsguardak4529.runasp.net/api';

// دالة عامة لإرسال طلبات
async function apiRequest(endpoint, method = 'GET', data = null) {
  const token = localStorage.getItem('token');
  const headers = {
    'Content-Type': 'application/json'
  };
  
  if (token) {
    headers['Authorization'] = `Bearer ${token}`;
  }
  
  const options = {
    method,
    headers,
    credentials: 'include'
  };
  
  if (data && (method === 'POST' || method === 'PUT')) {
    options.body = JSON.stringify(data);
  }
  
  try {
    const response = await fetch(`${API_BASE_URL}${endpoint}`, options);
    
    if (!response.ok) {
      throw new Error(`خطأ في الطلب: ${response.status} ${response.statusText}`);
    }
    
    // التحقق مما إذا كانت الاستجابة تحتوي على بيانات JSON
    const contentType = response.headers.get('content-type');
    if (contentType && contentType.includes('application/json')) {
      return await response.json();
    }
    
    return await response.text();
  } catch (error) {
    console.error('خطأ في طلب API:', error);
    throw error;
  }
}

// تصدير الدوال
window.api = {
  get: (endpoint) => apiRequest(endpoint, 'GET'),
  post: (endpoint, data) => apiRequest(endpoint, 'POST', data),
  put: (endpoint, data) => apiRequest(endpoint, 'PUT', data),
  delete: (endpoint) => apiRequest(endpoint, 'DELETE')
};
```

### 2. وحدة المصادقة (auth.js)

```javascript
// وحدة للتعامل مع المصادقة
const auth = {
  // تسجيل الدخول
  login: async (idNumber, password) => {
    try {
      const data = await window.api.post('/Account/Login', { idNumber, password });
      
      // تخزين بيانات المستخدم ورمز المصادقة
      localStorage.setItem('token', data.token);
      localStorage.setItem('user', JSON.stringify({
        id: data.id,
        name: data.name,
        idNumber: data.idNumber,
        email: data.email,
        counterId: data.counterId
      }));
      
      return data;
    } catch (error) {
      console.error('خطأ في تسجيل الدخول:', error);
      throw error;
    }
  },
  
  // تسجيل مستخدم جديد
  register: async (userData) => {
    try {
      const result = await window.api.post('/Account/RegisterAsUser', userData);
      return result;
    } catch (error) {
      console.error('خطأ في تسجيل المستخدم:', error);
      throw error;
    }
  },
  
  // التحقق من حالة تسجيل الدخول
  isLoggedIn: () => {
    return !!localStorage.getItem('token');
  },
  
  // الحصول على بيانات المستخدم
  getUser: () => {
    const user = localStorage.getItem('user');
    return user ? JSON.parse(user) : null;
  },
  
  // تسجيل الخروج
  logout: () => {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    window.location.href = 'Login.html';
  }
};

// تصدير الوحدة
window.auth = auth;
```

### 3. وحدة بيانات العداد (counter.js)

```javascript
// وحدة للتعامل مع بيانات العداد
const counter = {
  // الحصول على بيانات العداد
  getCounterData: async (counterId) => {
    try {
      return await window.api.get(`/Counter/GetCounterData/${counterId}`);
    } catch (error) {
      console.error('خطأ في جلب بيانات العداد:', error);
      throw error;
    }
  },
  
  // التحقق مما إذا كان المستخدم سارقًا
  isUserThief: async (counterId) => {
    try {
      return await window.api.get(`/Counter/IsUserThief/${counterId}`);
    } catch (error) {
      console.error('خطأ في التحقق من سرقة المستخدم:', error);
      throw error;
    }
  },
  
  // إرسال قراءة من الأردوينو
  processArduinoReading: async (readingData) => {
    try {
      return await window.api.post('/Counter/ProcessArduinoReading', readingData);
    } catch (error) {
      console.error('خطأ في إرسال قراءة الأردوينو:', error);
      throw error;
    }
  }
};

// تصدير الوحدة
window.counter = counter;
```

### 4. وحدة حسابات الكهرباء (electricity.js)

```javascript
// وحدة للتعامل مع حسابات الكهرباء
const electricity = {
  // حساب استهلاك الكهرباء
  calculate: async (deviceData, season = 'summer') => {
    try {
      return await window.api.post(`/Electricity/calculate?season=${season}`, deviceData);
    } catch (error) {
      console.error('خطأ في حساب استهلاك الكهرباء:', error);
      throw error;
    }
  },
  
  // الحصول على تفاصيل موديل معين
  getModelDetails: async (model) => {
    try {
      return await window.api.get(`/Electricity/model-details?model=${model}`);
    } catch (error) {
      console.error('خطأ في جلب تفاصيل الموديل:', error);
      throw error;
    }
  }
};

// تصدير الوحدة
window.electricity = electricity;
```

## خطوات التنفيذ

1. إنشاء هيكل المشروع وتنظيم الملفات
2. تطوير وحدات JavaScript المشتركة
3. تحديث كل صفحة HTML لاستخدام وحدات API المناسبة
4. اختبار كل وظيفة للتأكد من عملها بشكل صحيح
5. معالجة الأخطاء وتحسين تجربة المستخدم
6. تجميع المشروع النهائي للنشر

## ملاحظات إضافية

- سيتم استخدام localStorage لتخزين رمز المصادقة JWT وبيانات المستخدم
- سيتم التحقق من صلاحيات المستخدم قبل الوصول إلى صفحات الإدارة
- سيتم إضافة معالجة الأخطاء المناسبة لجميع طلبات API
- سيتم إضافة مؤشرات تحميل لتحسين تجربة المستخدم
