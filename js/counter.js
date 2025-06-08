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
