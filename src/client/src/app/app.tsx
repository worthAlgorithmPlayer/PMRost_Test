import { useState } from 'react';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { TimesheetPage } from '@/pages/timesheet/ui/timesheet-page';
import { ProjectReportPage } from '@/pages/project-report-page/ui/project-repost-page';

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      refetchOnWindowFocus: false,
      retry: 1,
    },
  },
});

export const App = () => {
  const [activeTab, setActiveTab] = useState<'timesheet' | 'report'>('timesheet');

  return (
    <QueryClientProvider client={queryClient}>
      <div style={{ fontFamily: 'sans-serif', padding: '20px', maxWidth: '1200px', margin: '0 auto' }}>
        <header style={{ display: 'flex', gap: '10px', marginBottom: '20px', borderBottom: '1px solid #ccc', paddingBottom: '10px' }}>
          <button 
            style={{ fontWeight: activeTab === 'timesheet' ? 'bold' : 'normal' }}
            onClick={() => setActiveTab('timesheet')}
          >
            ЭКРАН 1: Табель
          </button>
          <button 
            style={{ fontWeight: activeTab === 'report' ? 'bold' : 'normal' }}
            onClick={() => setActiveTab('report')}
          >
            ЭКРАН 2: Отчёт по проектам
          </button>
        </header>

        <main>
          {activeTab === 'timesheet' ? <TimesheetPage /> : <ProjectReportPage />}
        </main>
      </div>
    </QueryClientProvider>
  );
};

export default App;