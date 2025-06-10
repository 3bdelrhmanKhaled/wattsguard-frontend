// وحدة للتعامل مع طلبات API
const API_BASE_URL = 'http://wattsguardak4529.runasp.net/api';

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
    credentials: 'include',
    mode: 'cors',
    cache: 'no-cache',
    referrerPolicy: 'no-referrer'
  };
  
  if (data && (method === 'POST' || method === 'PUT')) {
    if (data instanceof FormData) {
      delete headers['Content-Type'];
      options.body = data;
    } else {
      options.body = JSON.stringify(data);
    }
  }
  
  try {
    console.log(`Sending ${method} request to ${API_BASE_URL}${endpoint}`, data);
    const response = await fetch(`${API_BASE_URL}${endpoint}`, options);
    
    console.log('Response status:', response.status);
    console.log('Response headers:', response.headers);
    
    const responseText = await response.text();
    console.log('Response text:', responseText);
    
    if (!responseText) {
      return {};
    }
    
    try {
      const jsonResponse = JSON.parse(responseText);
      if (!response.ok) {
        throw new Error(`خطأ في الطلب: ${response.status} ${response.statusText} - ${JSON.stringify(jsonResponse)}`);
      }
      return jsonResponse;
    } catch (jsonError) {
      if (!response.ok) {
        throw new Error(`خطأ في الطلب: ${response.status} ${response.statusText} - ${responseText}`);
      }
      return responseText;
    }
  } catch (error) {
    console.error('خطأ في طلب API:', error);
    throw error;
  }
}

window.api = {
  get: (endpoint) => apiRequest(endpoint, 'GET'),
  post: (endpoint, data) => apiRequest(endpoint, 'POST', data),
  put: (endpoint, data) => apiRequest(endpoint, 'PUT', data),
  delete: (endpoint) => apiRequest(endpoint, 'DELETE')
};