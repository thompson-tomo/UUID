<template>
  <div class="error-page">
    <div class="error-content">
      <div class="error-container">
        <div class="error-icon">
          <i class="fas fa-server pulse"></i>
        </div>
        <h1 class="glow">500</h1>
        <h2>Server Error</h2>
        <p>Sorry, something went wrong on our end. Please try again later.</p>
        <router-link to="/" class="home-button">
          <i class="fas fa-home"></i> Back to Home
        </router-link>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'ServerError'
}
</script>

<style scoped>
.error-page {
  position: fixed;
  top: 0;
  right: 0;
  bottom: 0;
  left: var(--sidebar-width);
  background-color: var(--bg-primary);
  overflow: auto;
}

.error-content {
  min-height: 100%;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 40px 20px;
}

.error-container {
  text-align: center;
  padding: 40px;
  background-color: var(--bg-secondary);
  border-radius: 8px;
  box-shadow: 0 8px 16px rgba(255, 215, 0, 0.1);
  width: 100%;
  max-width: 600px;
  animation: fadeIn 0.5s ease-out;
}

.error-icon {
  font-size: 4rem;
  color: #ffd700;
  margin-bottom: 2rem;
  display: inline-block;
}

.pulse {
  animation: pulse 2s infinite;
}

@keyframes pulse {
  0% {
    transform: scale(1);
  }
  50% {
    transform: scale(1.1);
  }
  100% {
    transform: scale(1);
  }
}

h1 {
  font-size: clamp(4rem, 10vw, 6rem);
  color: #ffd700;
  margin: 0;
  line-height: 1;
  font-weight: 700;
  background: linear-gradient(45deg, #ffd700, #ffed4a);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
}

.glow {
  animation: glow 2s ease-in-out infinite alternate;
}

@keyframes glow {
  from {
    text-shadow: 0 0 5px #fff, 0 0 10px #fff, 0 0 15px #ffd700, 0 0 20px #ffd700;
  }
  to {
    text-shadow: 0 0 10px #fff, 0 0 20px #fff, 0 0 30px #ffd700, 0 0 40px #ffd700;
  }
}

h2 {
  font-size: clamp(1.5rem, 4vw, 2rem);
  margin: 1rem 0;
  color: #fff;
}

p {
  color: #a7a7a7;
  font-size: 1.1rem;
  margin-bottom: 2rem;
  max-width: 600px;
  margin-left: auto;
  margin-right: auto;
}

.home-button {
  display: inline-block;
  padding: 12px 24px;
  background-color: #ffd700;
  color: #000;
  text-decoration: none;
  border-radius: 6px;
  font-weight: 500;
  transition: all 0.3s ease;
}

.home-button:hover {
  background-color: #ffed4a;
  transform: translateY(-2px);
}

.home-button i {
  font-size: 1.1rem;
}

@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(-20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

@media (max-width: 768px) {
  .error-page {
    left: 0;
  }

  .error-container {
    margin: 20px;
    padding: 20px;
  }

  .error-icon {
    font-size: 3rem;
    margin-bottom: 1rem;
  }

  h1 {
    font-size: clamp(3rem, 8vw, 4rem);
  }

  h2 {
    font-size: 1.5rem;
    margin: 10px 0;
  }

  p {
    font-size: 1rem;
    margin: 10px 0;
  }

  .home-button {
    padding: 10px 20px;
    font-size: 1rem;
  }
}
</style>