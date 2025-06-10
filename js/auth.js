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
      // تصحيح: استخدام الاسم الدقيق للحقول كما هو متوقع من الـ API
      const formattedData = {
        register: {
          idNumber: userData.idNumber,
          name: userData.name,
          password: userData.password,
          email: userData.email,
          phone: userData.phone,
          counterId: userData.counterId.toString(),
          address: {
            street: userData.address.street,
            region: userData.address.region,
            city: userData.address.city,
            governorate: userData.address.governorate
          }
        }
      };
      
      console.log('Sending registration data:', formattedData);
      const result = await window.api.post('/Account/RegisterAsUser', formattedData);
      return result;
    } catch (error) {
      console.error('خطأ في تسجيل المستخدم:', error);
      throw error;
    }
  },
  
  // تسجيل مستخدم جديد (نسخة محدثة)
  registerUser: async (userData) => {
    try {
      // تنسيق البيانات بالشكل المطلوب
      const formattedData = {
        register: {
          idNumber: userData.idNumber,
          name: userData.name,
          password: userData.password,
          email: userData.email,
          phone: userData.phone,
          counterId: userData.counterId.toString(),
          address: {
            street: userData.address.street,
            region: userData.address.region,
            city: userData.address.city,
            governorate: userData.address.governorate
          }
        }
      };
      
      console.log('Sending registration data:', formattedData);
      const result = await window.api.post('/Account/RegisterAsUser', formattedData);
      return result;
    } catch (error) {
      console.error('خطأ في تسجيل المستخدم:', error);
      throw error;
    }
  },
  
  // تسجيل مستخدم جديد (نسخة محدثة)
  registerUser: async (userData) => {
    try {
      // تنسيق البيانات بالشكل المطلوب
      const formattedData = {
        idNumber: userData.idNumber,
        name: userData.name,
        password: userData.password,
        email: userData.email,
        phone: userData.phone,
        counterId: userData.counterId.toString(),
        address: {
          street: userData.address.street,
          region: userData.address.region,
          city: userData.address.city,
          governorate: userData.address.governorate
        }
      };
      
      console.log('Sending registration data:', formattedData);
      const result = await window.api.post('/Account/RegisterAsUser', formattedData);
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
  
  // التحقق من صلاحيات المدير
  isAdmin: (user) => {
    return user && (user.counterId === '0' || user.counterId === 'Guard0');
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
