// وحدة للتعامل مع حسابات الكهرباء
const electricity = {
  calculate: async (deviceData, season = 'summer') => {
    try {
      return await window.api.post(`/Electricity/calculate?season=${season}`, deviceData);
    } catch (error) {
      console.error('خطأ في حساب استهلاك الكهرباء:', error);
      throw error;
    }
  },
  
  getModelDetails: async (model) => {
    try {
      return await window.api.get(`/Electricity/model-details?model=${model}`);
    } catch (error) {
      console.error('خطأ في جلب تفاصيل الموديل:', error);
      throw error;
    }
  }
};

window.electricity = electricity;